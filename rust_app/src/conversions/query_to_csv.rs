extern crate csv; // `csv`クレートを外部クレートとしてインポート

use std::ffi::{CStr, CString}; // C言語の文字列をRustで扱うための型をインポート
use std::os::raw::c_char;       // C言語の `char` 型をインポート
use csv::Writer;                // `csv::Writer` をインポートしてCSV書き込みをサポート

#[no_mangle]
pub extern "C" fn query_to_csv(
    headers: *const c_char, // カンマ区切りのヘッダー文字列
    data: *const c_char     // カンマ区切りのデータ文字列（複数行）
) -> *mut c_char {
    // ヘッダー文字列を取得（C言語からの文字列をRustの文字列へ変換）
    let headers_str = unsafe { CStr::from_ptr(headers).to_str().unwrap() };

    // データ文字列を取得（C言語からの文字列をRustの文字列へ変換）
    let data_str = unsafe { CStr::from_ptr(data).to_str().unwrap() };

    // CSVライターを作成（ベクタにCSVデータを書き込む）
    let mut wtr = Writer::from_writer(vec![]);

    // ヘッダーをCSVに書き込み
    let header_vec: Vec<&str> = headers_str.split(',').collect();
    if let Err(_) = wtr.write_record(&header_vec) {
        return std::ptr::null_mut(); // エラーが発生した場合は null ポインタを返す
    }

    // 各行のデータをCSVに書き込み
    for line in data_str.lines() {
        let record: Vec<&str> = line.split(',').collect();
        if let Err(_) = wtr.write_record(&record) {
            return std::ptr::null_mut(); // エラーが発生した場合は null ポインタを返す
        }
    }

    // CSVデータを取得
    let csv_data = match wtr.into_inner() { // `into_inner` メソッドを使用してCSVデータを取得
        Ok(data) => data,                   // 成功した場合は `data` を取得
        Err(_) => return std::ptr::null_mut(), // エラーが発生した場合は null ポインタを返す
    };

    // CSVデータをCStringに変換し、C言語のポインタとして返す
    let csv_c_string = CString::new(csv_data).unwrap();
    csv_c_string.into_raw()
}