
# データ変換ツール仕様書（C# + Rust）

## 機能概要

このツールは、CSV、JSON、およびSQL `INSERT` 文の相互変換を提供します。具体的には、以下の操作をサポートします。

### CSV ⇔ JSON

- **CSVデータをJSONフォーマットに変換**:
  - CSVファイルを読み込み、各行をJSON形式のオブジェクトに変換します。

- **JSONデータをCSVフォーマットに変換**:
  - JSONデータを読み込み、各オブジェクトをCSV形式に変換します。

### CSV ⇔ SQL（INSERT文）

- **CSVファイルからSQLのINSERT文を生成**:
  - CSVファイルの各行を基に、指定されたテーブルに対する`INSERT`文を生成します。
  - 例: `INSERT INTO table_name (column1, column2) VALUES ('value1', 'value2');`

- **SQLのINSERT文からCSVに変換**:
  - SQL `INSERT`文を解析し、CSV形式に変換します。

### JSON ⇔ SQL（INSERT文）

- **JSONデータからSQLのINSERT文を生成**:
  - JSONデータを基に、指定されたテーブルに対する`INSERT`文を生成します。

- **SQLのINSERT文からJSONに変換**:
  - SQL `INSERT`文を解析し、JSON形式に変換します。

## 要件

### 1. システム要件

- **プラットフォーム**:
  - Windows 10/11
  - C#を使用したWindowsアプリケーション
  - RustバックエンドをDLLとして利用

- **言語とツール**:
  - **フロントエンド**: C# (.NET 6+)
  - **バックエンド**: Rust (DLLとしてエクスポート)

### 2. 機能要件

#### a. CSV ⇔ JSON

- **CSVからJSONへの変換**:
  - CSVファイルを読み込み、各行をJSON形式のオブジェクトに変換します。
  - **サポートするデータ型**: 文字列、数値、ブール値。

- **JSONからCSVへの変換**:
  - JSONデータを読み込み、各オブジェクトをCSV形式に変換します。
  - **フィールド名**をCSVのヘッダーとして使用します。

#### b. CSV ⇔ SQL（INSERT文）

- **CSVからSQL（INSERT文）への変換**:
  - CSVファイルの各行を基に、指定されたテーブルに対する`INSERT`文を生成します。
  - 例: `INSERT INTO table_name (column1, column2) VALUES ('value1', 'value2');`

- **SQL（INSERT文）からCSVへの変換**:
  - SQL `INSERT`文を解析し、CSV形式に変換します。

#### c. JSON ⇔ SQL（INSERT文）

- **JSONからSQL（INSERT文）への変換**:
  - JSONデータを基に、指定されたテーブルに対する`INSERT`文を生成します。

- **SQL（INSERT文）からJSONへの変換**:
  - SQL `INSERT`文を解析し、JSON形式に変換します。

## ディレクトリ構成

```
.
├── rust_app
│   └── (Rustのソースコードなど)
└── c_sharp_app
    ├── c_sharp_app.csproj
    └── (C#のソースコードなど)
```

## コマンド

### Rustプロジェクトのビルドと実行

```bash
cd rust_app
cargo build
cargo run
```

### C#プロジェクトのビルドと実行

```bash
cd c_sharp_app
dotnet build
dotnet run
```


## インストールが必要なもの

### 1. .NET SDK

C#のプロジェクトをビルドするために、.NET SDKをインストールする必要があります。

#### **macOSの場合（Homebrewを使用）**

1. ターミナルを開き、以下のコマンドを実行して `.NET SDK` をインストールします。

```bash
brew install --cask dotnet-sdk
```

2. インストールが完了したら、以下のコマンドでインストールが成功したか確認します。

```bash
dotnet --version
```

#### **Windowsの場合**

1. [公式サイト](https://dotnet.microsoft.com/download)にアクセスし、**Windows用のインストーラ**をダウンロードします。
2. ダウンロードした `.exe` ファイルを実行して、画面の指示に従いインストールします。
3. インストール完了後、以下のコマンドでインストールが成功したか確認します。

```bash
dotnet --version
```

---

### 2. Rust

Rustを使用するために、Rustのツールチェーンをインストールします。

#### **macOSの場合（Homebrewを使用）**

1. ターミナルを開き、以下のコマンドを実行して `Rust` をインストールします。

```bash
brew install rustup
rustup-init
```

2. インストールが完了したら、以下のコマンドで環境を適用します。

```bash
source $HOME/.cargo/env
```

3. 次に、Rustのインストールを確認するため、以下のコマンドを実行します。

```bash
rustc --version
```

#### **Windowsの場合**

1. [Rustの公式インストーラ](https://rustup.rs/)にアクセスし、Windows用のインストーラをダウンロードします。
2. ダウンロードした `.exe` ファイルを実行して、インストーラの指示に従いRustをインストールします。
3. インストールが完了したら、コマンドプロンプトまたはPowerShellを開き、以下のコマンドでインストールを確認します。

```bash
rustc --version
```
