extern crate json;

use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;
use json::JsonValue;

/// JSONデータをSQL INSERTクエリに変換する関数
#[no_mangle]
pub extern "C" fn json_to_query(json_input: *const c_char) -> *mut c_char {
    // 処理開始時間の記録
    let start_time = Instant::now();

    // 入力のJSON文字列を取得
    let json_str = unsafe { CStr::from_ptr(json_input).to_str().unwrap() };

    // JSON文字列をパース
    let parsed_json: JsonValue = json::parse(json_str).unwrap();

    // クエリ生成用の文字列
    let mut sql_queries = String::new();

    // JSONデータのメンバーを処理
    for record in parsed_json.members() {
        let mut columns = Vec::new();
        let mut values = Vec::new();

        // 各エントリーを処理
        for (key, value) in record.entries() {
            columns.push(key.to_string());

            // 値をSQL用の文字列に変換
            let value_str = match value {
                JsonValue::String(s) => format!("'{}'", s.replace("'", "''")), // シングルクォートをエスケープ
                JsonValue::Number(n) => n.to_string(),
                JsonValue::Boolean(b) => b.to_string(),
                JsonValue::Null => "NULL".to_string(),
                _ => "".to_string(), // その他の型は空文字列に
            };
            values.push(value_str);
        }

        // INSERT文の生成
        let query = format!(
            "INSERT INTO table_name ({}) VALUES ({});",
            columns.join(", "),
            values.join(", ")
        );
        sql_queries.push_str(&query);
        sql_queries.push('\n');
    }

    // 処理終了時間の記録
    let elapsed_time = start_time.elapsed();
    println!("JSON → SQLクエリ 変換にかかった時間: {:?}", elapsed_time);

    // CString に変換してポインタを返す
    let query_c_string = CString::new(sql_queries).unwrap();
    query_c_string.into_raw()
}