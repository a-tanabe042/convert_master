extern crate csv;
extern crate sqlparser;

use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::ptr;
use csv::Writer;
use sqlparser::dialect::GenericDialect;
use sqlparser::parser::Parser;
use sqlparser::ast::{Statement, Expr, Value};

#[no_mangle]
pub extern "C" fn query_to_csv(
    query_data: *const c_char     // SQLクエリデータ
) -> *mut c_char {
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

    let mut headers: Vec<String> = Vec::new();
    let mut data_rows: Vec<Vec<String>> = Vec::new();

    for stmt in ast {
        if let Statement::Insert { table_name: _, columns, source, .. } = stmt {
            // カラム名を取得
            let col_names: Vec<String> = columns.iter().map(|c| c.value.clone()).collect();

            if headers.is_empty() {
                headers = col_names.clone();
            }

            // VALUES句からデータを抽出
            if let sqlparser::ast::SetExpr::Values(values) = &*source.body {
                for row in &values.0 {
                    let mut row_data: Vec<String> = Vec::new();
                    for expr in row {
                        let value_str = match expr {
                            Expr::Value(Value::SingleQuotedString(s)) => s.clone(),
                            Expr::Value(Value::Number(n, _)) => n.clone(),
                            Expr::Value(Value::Null) => String::from("NULL"),
                            Expr::Value(Value::Boolean(b)) => b.to_string(),
                            _ => {
                                eprintln!("サポートされていない値の種類: {:?}", expr);
                                return ptr::null_mut();
                            }
                        };
                        row_data.push(value_str);
                    }
                    data_rows.push(row_data);
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

    // CSVライターを作成（ベクタにCSVデータを書き込む）
    let mut wtr = Writer::from_writer(vec![]);

    // ヘッダーをCSVに書き込み
    if let Err(e) = wtr.write_record(&headers) {
        eprintln!("ヘッダーの書き込みに失敗しました: {}", e);
        return ptr::null_mut();
    }

    // データをCSVに書き込み
    for record in data_rows {
        if let Err(e) = wtr.write_record(&record) {
            eprintln!("データの書き込みに失敗しました: {}", e);
            return ptr::null_mut();
        }
    }

    // CSVデータを取得
    let csv_data = match wtr.into_inner() {
        Ok(data) => data,
        Err(e) => {
            eprintln!("CSVデータの取得に失敗しました: {}", e);
            return ptr::null_mut();
        }
    };

    // CSVデータをCStringに変換し、C言語のポインタとして返す
    let csv_c_string = match CString::new(csv_data) {
        Ok(s) => s,
        Err(e) => {
            eprintln!("CSVデータのCString変換に失敗しました: {}", e);
            return ptr::null_mut();
        }
    };

    csv_c_string.into_raw()
}