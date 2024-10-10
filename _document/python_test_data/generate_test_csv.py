# python generate_csv.py

import csv
import random
import string

# データ生成の設定
num_rows = 100000  # 10万人
csv_file_path = 'large_data.csv'

def random_string(length=10):
    """指定した長さのランダムな文字列を生成する"""
    return ''.join(random.choices(string.ascii_letters + string.digits, k=length))

def random_age():
    """ランダムな年齢を生成する"""
    return str(random.randint(1, 100))

def generate_csv(file_path, num_rows):
    """指定した数のレコードを持つCSVファイルを生成する"""
    with open(file_path, 'w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        writer.writerow(['id', 'name', 'age'])  # ヘッダー

        for i in range(1, num_rows + 1):
            writer.writerow([i, random_string(), random_age()])

# CSVファイルの生成
generate_csv(csv_file_path, num_rows)
print(f'{num_rows} 件のデータを含むCSVファイルが {csv_file_path} に生成されました。')
