extern crate csv;
extern crate json;
use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;
use json::JsonValue;
use std::collections::HashMap;

#[no_mangle]
pub extern "C" fn json_to_csv(json_input: *const c_char) -> *mut c_char {
    // 処理開始時間の記録
    let start_time = Instant::now();

    // 入力のJSON文字列を取得
    let json_str = unsafe { CStr::from_ptr(json_input).to_str().unwrap() };

    // JSON文字列をパース
    let parsed_json: JsonValue = json::parse(json_str).unwrap();

    // ヘッダーを保存するためのセットとCSV文字列
    let mut headers = HashMap::new();
    let mut csv_output = String::new();

    // 最初のループでヘッダーを抽出
    for record in parsed_json.members() {
        for (key, _) in record.entries() {
            let headers_len = headers.len();  // len() を事前に取得
            headers.entry(key).or_insert(headers_len);  // or_insert に渡す
        }
    }

    // ヘッダーを書き込み
    let header_row: Vec<&str> = headers.keys().map(|&key| key).collect();
    csv_output.push_str(&header_row.join(","));
    csv_output.push('\n');

    // 各レコードを順序に従ってCSVに変換
    for record in parsed_json.members() {
        let mut csv_row = vec![""; headers.len()];
        for (key, value) in record.entries() {
            if let Some(&index) = headers.get(key) {
                csv_row[index] = value.as_str().unwrap_or("");
            }
        }
        csv_output.push_str(&csv_row.join(","));
        csv_output.push('\n');
    }

    // 処理終了時間の記録
    let elapsed_time = start_time.elapsed();
    println!("JSON → CSV 変換にかかった時間: {:?}", elapsed_time);

    // CString に変換してポインタを返す
    let csv_c_string = CString::new(csv_output).unwrap();
    csv_c_string.into_raw()
}