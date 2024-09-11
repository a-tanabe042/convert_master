2024/9/11

[内容]
csv → json
test data: 100,000人

[環境]
mac os: M1
memory: 16GB

[file]
large_data.csv
size: 2.0M

## 10万人のユーザーデータCSV → JSON 変換にかかった時間:

[結果]
全体の処理時間: 0.1292秒 (C#とRust)
CSV → JSON 変換にかかった時間: 0.047549292秒 (Rustのみ)

[比較]
CSV → JSON 変換にかかった時間: 0.36466097831726074秒 (Python)