extern crate csv;

use csv::Reader;
use std::ffi::{CStr, CString};
use std::os::raw::c_char;

#[no_mangle]
pub extern "C" fn csv_to_query(
    csv_file: *const c_char,
    table_name: *const c_char
) -> *mut c_char {
    // 入力のCSV文字列を取得
    let csv_str = unsafe { CStr::from_ptr(csv_file).to_str().unwrap() };

    // テーブル名の取得
    let table_name_str = unsafe { CStr::from_ptr(table_name).to_str().unwrap() };

    // CSVリーダーの作成
    let mut reader = Reader::from_reader(csv_str.as_bytes());

    // let headers = reader.headers()?.clone();
    // headers() のエラーハンドリングを match で行う
    let headers = match reader.headers() {
        Ok(h) => h.clone(),
        Err(_) => {
            // エラーが発生した場合は null ポインタを返す
            return std::ptr::null_mut();
        }
    };

    let mut sql_statements = String::new();

    for result in reader.records() {
        // let record = result.map_err(|e| format!("Error reading record: {}", e));
        let record = result.expect("Failed to read record");
        // `record.iter()` の各要素を `&str` に変換してから `replace` メソッドを使用
        let values: Vec<String> = record.iter()
            .map(|v| {
                let value_str = std::str::from_utf8(v.as_bytes()).expect("Invalid UTF-8");
                format!("'{}'", value_str.replace("'", "''"))
            })
            .collect();

        let sql = format!(
            "INSERT INTO {} ({}) VALUES ({});",
            table_name_str,
            headers.iter().collect::<Vec<&str>>().join(", "),
            values.join(", ")
        );
        
        sql_statements.push_str(&sql);
        sql_statements.push('\n');
    }

    // CString に変換してポインタを返す
    let c_string = CString::new(sql_statements).unwrap();
    c_string.into_raw()
}
