import csv
import json
import time

def csv_to_json(csv_file_path, json_file_path):
    """CSVファイルをJSONファイルに変換する"""
    data = []

    # CSVファイルを読み込む
    with open(csv_file_path, 'r', encoding='utf-8') as csv_file:
        reader = csv.DictReader(csv_file)

        # 各行を辞書としてリストに追加
        for row in reader:
            data.append(row)

    # JSONファイルに書き込む
    with open(json_file_path, 'w', encoding='utf-8') as json_file:
        json.dump(data, json_file, ensure_ascii=False, indent=4)

# 実行時間の測定
csv_file_path = 'large_data.csv'
json_file_path = 'large_data.json'

start_time = time.time()  # 開始時間

csv_to_json(csv_file_path, json_file_path)

end_time = time.time()  # 終了時間

execution_time = end_time - start_time
print(f'{csv_file_path} を {json_file_path} に変換しました。')
print(f'実行時間: {execution_time} 秒')