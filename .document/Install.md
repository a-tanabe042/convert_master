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
