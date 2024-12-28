extern crate json;

use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;
use json::JsonValue;

/// JSONデータをSQL INSERTクエリに変換する関数
#[no_mangle]
pub extern "C" fn json_to_query(json_input: *const c_char) -> *mut c_char {
    let start_time = Instant::now();

    let json_str = unsafe { CStr::from_ptr(json_input).to_str().unwrap() };
    let parsed_json: JsonValue = match json::parse(json_str) {
        Ok(json) => json,
        Err(e) => {
            eprintln!("Failed to parse JSON: {:?}", e);
            return CString::new("").unwrap().into_raw();
        }
    };

    let mut sql_queries = String::new();

    for record in parsed_json.members() {
        let mut columns = Vec::new();
        let mut values = Vec::new();

        for (key, value) in record.entries() {
            columns.push(key.to_string());

            // 値を適切に取得
            let value_str = match value {
                JsonValue::String(s) => format!("'{}'", s.replace("'", "''")), // エスケープ
                JsonValue::Number(n) => n.to_string(),
                JsonValue::Boolean(b) => b.to_string(),
                JsonValue::Short(s) => format!("'{}'", s.as_str().replace("'", "''")), // Shortを文字列として処理
                JsonValue::Null => "NULL".to_string(),
                _ => {
                    eprintln!("Unsupported value type: {:?}", value);
                    "".to_string()
                }
            };
            values.push(value_str);
        }

        if columns.is_empty() || values.is_empty() {
            eprintln!("Empty columns or values for record: {:?}", record);
            continue;
        }

        let query = format!(
            "INSERT INTO table_name ({}) VALUES ({});",
            columns.join(", "),
            values.join(", ")
        );
        sql_queries.push_str(&query);
        sql_queries.push('\n');
    }

    let elapsed_time = start_time.elapsed();
    println!("JSON → SQLクエリ 変換にかかった時間: {:?}", elapsed_time);

    CString::new(sql_queries).unwrap().into_raw()
}