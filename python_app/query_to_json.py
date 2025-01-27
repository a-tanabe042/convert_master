import os
import time
import json

def query_to_json(query_file_path, json_file_path):
    """SQLクエリをJSONファイルに変換する"""
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

            # ヘッダーと値を結びつける
            row = {header: value for header, value in zip(headers, values)}
            data.append(row)

    # データを書き込む
    with open(json_file_path, 'w', encoding='utf-8') as json_file:
        json.dump(data, json_file, ensure_ascii=False, indent=4)


# 入力と出力のパス
input_dir = os.path.join("data", "input")
output_dir = os.path.join("data", "output")
os.makedirs(output_dir, exist_ok=True)

query_file_path = os.path.join(input_dir, "large_data.sql")
json_file_path = os.path.join(output_dir, "converted_from_query.json")

# 処理時間の測定
start_time = time.time()
query_to_json(query_file_path, json_file_path)
end_time = time.time()

execution_time = end_time - start_time
print(f'{query_file_path} を {json_file_path} に変換しました。')
print(f'処理時間: {execution_time:.6f} 秒')