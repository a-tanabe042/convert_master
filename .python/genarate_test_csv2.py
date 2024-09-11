import csv
import random
import string

# データ生成の設定
num_rows = 100  # 100件のデータ
csv_file_path = 'japanese_test_data.csv'

# ランダムな文字列生成
def random_string(length=10):
    """指定した長さのランダムな文字列を生成する（英数字）"""
    return ''.join(random.choices(string.ascii_letters + string.digits, k=length))

# ランダムな日本語の名前生成
def random_japanese_name():
    """ランダムな日本語の名前を生成する"""
    family_names = ['佐藤', '鈴木', '高橋', '田中', '伊藤']
    given_names = ['太郎', '次郎', '花子', '美咲', '健一']
    return f"{random.choice(family_names)} {random.choice(given_names)}"

# ランダムな年齢生成
def random_age():
    """ランダムな年齢を生成する"""
    return str(random.randint(1, 100))

# ランダムな住所生成（日本語を含む）
def random_address():
    """ランダムな日本の住所を生成する"""
    cities = ['東京', '大阪', '京都', '福岡', '札幌']
    streets = ['中央区', '北区', '西区', '南区', '東区']
    return f"{random.choice(cities)} {random.choice(streets)}"

# CSVデータの生成
def generate_csv(file_path, num_rows):
    """指定した数のレコードを持つCSVファイルを生成する"""
    with open(file_path, 'w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        writer.writerow(['id', 'name', 'age', 'address'])  # ヘッダー

        for i in range(1, num_rows + 1):
            writer.writerow([i, random_japanese_name(), random_age(), random_address()])

# CSVファイルの生成
generate_csv(csv_file_path, num_rows)
print(f'{num_rows} 件のデータを含むCSVファイルが {csv_file_path} に生成されました。')