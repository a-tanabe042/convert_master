2024/9/11

[内容]
csv → json
test data: 1,000,000人

[環境]
mac os: M1
memory: 16GB

[結果]
10万人のユーザーデータCSV → JSON 変換にかかった時間:
全体の処理時間: 0.1292秒 (C#とRust)
CSV → JSON 変換にかかった時間: 0.047549292秒 (Rustのみ)

[バグ]
JSON dataがアルファベット順にソートされる。