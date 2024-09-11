extern crate csv;
extern crate serde_json;
use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::time::Instant;

#[no_mangle]
pub extern "C" fn csv_to_json(csv_input: *const c_char) -> *mut c_char {
    // 処理開始時間の記録
    let start_time = Instant::now();

    // 入力のCSV文字列を取得
    let csv_str = unsafe { CStr::from_ptr(csv_input).to_str().unwrap() };

    // CSVリーダーの作成
    let mut reader = csv::Reader::from_reader(csv_str.as_bytes());
    let headers = reader.headers().unwrap().clone(); // ヘッダーを取得

    let mut json_records = Vec::new(); // 結果として出力するレコードを保持

    // 各レコードを処理
    for result in reader.records() {
        let record = result.unwrap();
        let mut json_object = serde_json::Map::new();

        // フィールドをヘッダーの順序に従って追加
        for (header, field) in headers.iter().zip(record.iter()) {
            json_object.insert(header.to_string(), serde_json::Value::String(field.to_string()));
        }

        // JSONオブジェクトをリストに追加
        json_records.push(serde_json::Value::Object(json_object));
    }

    // JSON文字列に変換
    let json_output = serde_json::to_string(&json_records).unwrap();
    let json_c_string = CString::new(json_output).unwrap();

    // 処理終了時間の記録
    let elapsed_time = start_time.elapsed();

    // 実行時間をログに出力（ここでは標準出力に出力していますが、必要に応じて他の方法でログを記録できます）
    println!("CSV → JSON 変換にかかった時間: {:?}", elapsed_time);

    // ポインタを返す
    json_c_string.into_raw()
}
