extern crate sqlparser;
extern crate json;

use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;
use std::ptr;
use sqlparser::dialect::GenericDialect;
use sqlparser::parser::Parser;
use sqlparser::ast::{Statement, Expr, Value};
use json::JsonValue;

#[no_mangle]
pub extern "C" fn query_to_json(query_data: *const c_char) -> *mut c_char {
    // 処理開始時間の記録
    let start_time = Instant::now();

    // クエリ文字列を取得（C言語からの文字列をRustの文字列へ変換）
    let query_str = match unsafe { CStr::from_ptr(query_data).to_str() } {
        Ok(s) => s,
        Err(e) => {
            eprintln!("クエリデータのUTF-8変換に失敗しました: {}", e);
            return ptr::null_mut();
        }
    };

    // SQLクエリをパース
    let dialect = GenericDialect {}; // SQLの方言を指定
    let ast = match Parser::parse_sql(&dialect, query_str) {
        Ok(ast) => ast,
        Err(e) => {
            eprintln!("SQLクエリのパースに失敗しました: {}", e);
            return ptr::null_mut();
        }
    };

    let mut records = JsonValue::new_array();
    let mut headers: Vec<String> = Vec::new();

    // ASTを解析してデータを抽出
    for stmt in ast {
        if let Statement::Insert { columns, source, .. } = stmt {
            // カラム名を取得
            let col_names: Vec<String> = columns.iter().map(|c| c.value.clone()).collect();
            if headers.is_empty() {
                headers = col_names.clone();
            }

            // VALUES句からデータを抽出
            if let sqlparser::ast::SetExpr::Values(values) = &*source.body {
                for row in &values.0 {
                    let mut record = JsonValue::new_object();
                    for (i, expr) in row.iter().enumerate() {
                        let value = match expr {
                            Expr::Value(Value::SingleQuotedString(s)) => JsonValue::String(s.clone()),
                            Expr::Value(Value::Number(n, _)) => match n.parse::<f64>() {
                                Ok(num) => JsonValue::Number(num.into()),
                                Err(_) => {
                                    eprintln!("数値のパースに失敗しました: {}", n);
                                    return ptr::null_mut();
                                }
                            },
                            Expr::Value(Value::Null) => JsonValue::Null,
                            Expr::Value(Value::Boolean(b)) => JsonValue::Boolean(*b),
                            _ => {
                                eprintln!("サポートされていない値の種類: {:?}", expr);
                                return ptr::null_mut();
                            }
                        };
                        let column_name = headers.get(i).unwrap_or(&format!("column{}", i + 1)).clone();
                        record[column_name] = value;
                    }
                    records.push(record).unwrap();
                }
            } else {
                eprintln!("サポートされていないクエリタイプです");
                return ptr::null_mut();
            }
        } else {
            eprintln!("サポートされていないステートメントです");
            return ptr::null_mut();
        }
    }

    // JSONデータを文字列にシリアライズ
    let json_data = records.dump();

    // 処理終了時間の記録
    let elapsed_time = start_time.elapsed();
    println!("SQL → JSON 変換にかかった時間: {:?}", elapsed_time);

    // JSONデータをCStringに変換し、C言語のポインタとして返す
    let json_c_string = match CString::new(json_data) {
        Ok(s) => s,
        Err(e) => {
            eprintln!("JSONデータのCString変換に失敗しました: {}", e);
            return ptr::null_mut();
        }
    };

    json_c_string.into_raw()
}
