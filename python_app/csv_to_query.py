import csv
import os
import time  # 処理時間を測定するために追加

def csv_to_query(csv_file_path, query_file_path, table_name):
    """CSVファイルをSQLクエリに変換する"""
    queries = []

    with open(csv_file_path, 'r', encoding='utf-8') as csv_file:
        reader = csv.DictReader(csv_file)
        headers = reader.fieldnames  # CSVのヘッダーを取得

        # INSERT文のベース
        insert_statement = f"INSERT INTO {table_name} ({', '.join(headers)}) VALUES"

        for row in reader:
            # 各行の値をSQL用にエスケープして追加
            sanitized_values = []
            for value in row.values():
                # シングルクォートを2つにエスケープ
                sanitized_value = value.replace("'", "''")
                sanitized_values.append(f"'{sanitized_value}'")
            values = ', '.join(sanitized_values)
            queries.append(f"{insert_statement} ({values});")

    # クエリをファイルに保存
    with open(query_file_path, 'w', encoding='utf-8') as query_file:
        query_file.write('\n'.join(queries))


# 入力ファイルと出力ファイルのパス
input_dir = os.path.join("data", "input")
output_dir = os.path.join("data", "output", "query_output")
os.makedirs(output_dir, exist_ok=True)

csv_file_path = os.path.join(input_dir, "large_data.csv")
query_file_path = os.path.join(output_dir, "large_data.sql")

# 処理時間の測定
start_time = time.time()  # 処理開始時間

# CSV → SQLクエリ変換
csv_to_query(csv_file_path, query_file_path, "SampleTbl")

end_time = time.time()  # 処理終了時間
execution_time = end_time - start_time  # 処理時間を計算

print(f'{csv_file_path} を {query_file_path} に変換しました。')
print(f'処理時間: {execution_time:.6f} 秒')