extern crate json;

use std::collections::HashSet;
use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;
use json::JsonValue;

#[no_mangle]
pub extern "C" fn json_to_csv(json_input: *const c_char) -> *mut c_char {
    // 処理開始時間の記録
    let start_time = Instant::now();

    // C 文字列から Rust 文字列へ変換 (unsafe)
    let json_str = unsafe { CStr::from_ptr(json_input).to_str().unwrap() };

    // JSON文字列をパース
    let parsed_json: JsonValue = json::parse(json_str).unwrap();

    // -----------------------------
    // 1. 全レコードからキーを「初回出現順」で収集
    // -----------------------------
    let mut headers = Vec::new();
    let mut seen_keys = HashSet::new(); // 登録済みキーの管理用

    // parsed_json が配列である前提
    for record in parsed_json.members() {
        // 1レコードの全エントリ ( key, value ) を走査
        for (key, _) in record.entries() {
            // まだ見たことのないキーなら、ヘッダーリストに追加
            if !seen_keys.contains(key) {
                seen_keys.insert(key.to_string());
                headers.push(key.to_string());
            }
        }
    }

    // -----------------------------
    // 2. CSV 文字列を作成
    // -----------------------------
    let mut csv_output = String::new();

    // 2-1. ヘッダー行の書き込み (初回出現順)
    csv_output.push_str(&headers.join(","));
    csv_output.push('\n');

    // 2-2. 各レコードの書き込み
    for record in parsed_json.members() {
        let mut row_values = Vec::new();
        for header in &headers {
            // JSON 上で key = header の要素を取り出す
            // 文字列でない場合やキーが存在しない場合は空文字 "" にする
            let value = record[header].as_str().unwrap_or("");
            row_values.push(value);
        }
        csv_output.push_str(&row_values.join(","));
        csv_output.push('\n');
    }

    // 処理終了時間の記録
    let elapsed_time = start_time.elapsed();
    println!("JSON → CSV 変換にかかった時間: {:?}", elapsed_time);

    // -----------------------------
    // 3. CString に変換してポインタを返す
    // -----------------------------
    let csv_c_string = CString::new(csv_output).unwrap();
    csv_c_string.into_raw()
}