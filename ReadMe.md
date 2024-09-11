
# 爆速データ変換ツール仕様書（C# + Rust）

## 目的
・生涯、使えるサービスを作る。
・コンパイラ言語の知見を深める。

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