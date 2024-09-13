# ベースイメージとしてRustの公式イメージを使用
FROM rust:latest

# 作業ディレクトリの設定
WORKDIR /usr/src/myapp

# カレントディレクトリの内容をコンテナの作業ディレクトリにコピー
COPY . .

# 依存関係をインストール（キャッシュを効率的に利用するためのステップ）
RUN cargo install --path .

# アプリケーションをビルド
RUN cargo build --release

# 実行ファイルを実行
CMD ["./target/release/myapp"]