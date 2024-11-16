extern crate csv;
extern crate json;

use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;
use json::JsonValue;

#[no_mangle]
pub extern "C" fn csv_to_json(csv_input: *const c_char) -> *mut c_char {
    // 処理開始時間の記録
    let start_time = Instant::now();

    // 入力のCSV文字列を取得
    let csv_str = unsafe { CStr::from_ptr(csv_input).to_str().unwrap() };

    // CSVリーダーの作成
    let mut reader = csv::Reader::from_reader(csv_str.as_bytes());
    let headers = reader.headers().unwrap().clone(); // ヘッダーを取得

    let mut json_records = JsonValue::new_array(); // JSON 配列

    // 各レコードを処理
    for result in reader.records() {
        let record = result.unwrap();

        // フィールドを順序付きで保持するためにオブジェクトを作成
        let mut json_object = JsonValue::new_object();
        for (header, field) in headers.iter().zip(record.iter()) {
            json_object[header] = field.into();
        }

        // JSON オブジェクトを配列に追加
        json_records.push(json_object).unwrap();
    }

    // JSON文字列に変換
    let json_output = json::stringify_pretty(json_records, 4);

    let json_c_string = CString::new(json_output).unwrap();

    // 処理終了時間の記録
    let elapsed_time = start_time.elapsed();
    println!("CSV → JSON 変換にかかった時間: {:?}", elapsed_time);

    // ポインタを返す
    json_c_string.into_raw()
}