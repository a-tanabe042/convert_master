import json
import os
import time

def json_to_query(json_file_path, query_file_path, table_name):
    """JSONファイルをSQLクエリに変換する"""
    with open(json_file_path, 'r', encoding='utf-8') as json_file:
        data = json.load(json_file)

    # クエリを生成
    queries = []
    for row in data:
        columns = ', '.join(row.keys())

        # 各値をエスケープ処理
        sanitized_values = []
        for value in row.values():
            # シングルクォートをエスケープ
            sanitized_value = str(value).replace("'", "''")
            sanitized_values.append(f"'{sanitized_value}'")

        values = ', '.join(sanitized_values)
        query = f"INSERT INTO {table_name} ({columns}) VALUES ({values});"
        queries.append(query)

    # SQLクエリをファイルに書き込む
    with open(query_file_path, 'w', encoding='utf-8') as query_file:
        query_file.write('\n'.join(queries))


# 入力と出力のパス
input_dir = os.path.join("data", "input")
output_dir = os.path.join("data", "output")
os.makedirs(output_dir, exist_ok=True)

json_file_path = os.path.join(input_dir, "large_data.json")
query_file_path = os.path.join(output_dir, "converted_data.sql")

# 処理時間の測定
start_time = time.time()
json_to_query(json_file_path, query_file_path, "SampleTbl")
end_time = time.time()

execution_time = end_time - start_time
print(f'{json_file_path} を {query_file_path} に変換しました。')
print(f'処理時間: {execution_time:.6f} 秒')