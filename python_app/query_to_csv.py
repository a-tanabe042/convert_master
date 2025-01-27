import os
import time
import csv

def query_to_csv(query_file_path, csv_file_path):
    """SQLクエリをCSVファイルに変換する"""
    with open(query_file_path, 'r', encoding='utf-8') as query_file:
        queries = query_file.readlines()

    headers = None
    data = []

    for query in queries:
        if query.strip().upper().startswith("INSERT INTO"):
            # ヘッダー解析
            if headers is None:
                headers_start = query.index("(") + 1
                headers_end = query.index(")")
                headers = [h.strip() for h in query[headers_start:headers_end].split(",")]

            # 値解析
            values_start = query.upper().index("VALUES") + len("VALUES")
            values_part = query[values_start:].strip().strip("();")
            values = [v.strip("'") for v in values_part.split(",")]
            data.append(values)

    # データを書き込む
    if headers is not None:
        with open(csv_file_path, 'w', encoding='utf-8', newline='') as csv_file:
            writer = csv.writer(csv_file)
            writer.writerow(headers)
            writer.writerows(data)


# 入力と出力のパス
input_dir = os.path.join("data", "input")
output_dir = os.path.join("data", "output")
os.makedirs(output_dir, exist_ok=True)

query_file_path = os.path.join(input_dir, "large_data.sql")
csv_file_path = os.path.join(output_dir, "converted_from_query.csv")

# 処理時間の測定
start_time = time.time()
query_to_csv(query_file_path, csv_file_path)
end_time = time.time()

execution_time = end_time - start_time
print(f'{query_file_path} を {csv_file_path} に変換しました。')
print(f'処理時間: {execution_time:.6f} 秒')