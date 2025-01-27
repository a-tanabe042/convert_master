import csv
import json
import os
import time

def json_to_csv(json_file_path, csv_file_path):
    """JSONファイルをCSVファイルに変換する"""
    with open(json_file_path, 'r', encoding='utf-8') as json_file:
        data = json.load(json_file)

    # ヘッダーを取得
    headers = data[0].keys()

    # CSVファイルに書き込む
    with open(csv_file_path, 'w', encoding='utf-8', newline='') as csv_file:
        writer = csv.DictWriter(csv_file, fieldnames=headers)
        writer.writeheader()
        writer.writerows(data)


# 入力と出力のパス
input_dir = os.path.join("data", "input")
output_dir = os.path.join("data", "output")
os.makedirs(output_dir, exist_ok=True)

json_file_path = os.path.join(input_dir, "large_data.json")
csv_file_path = os.path.join(output_dir, "converted_data.csv")

# 処理時間の測定
start_time = time.time()
json_to_csv(json_file_path, csv_file_path)
end_time = time.time()

execution_time = end_time - start_time
print(f'{json_file_path} を {csv_file_path} に変換しました。')
print(f'処理時間: {execution_time:.6f} 秒')