
## 1. Rustライブラリの作成

**--Cargo.toml の設定--**

まず、Rustプロジェクトの `Cargo.toml` ファイルを次のように設定します。

```toml
[package]
name = "rust_app"
version = "0.1.0"
edition = "2021"

[lib]
crate-type = ["cdylib"]

[dependencies]
```

`crate-type = ["cdylib"]`: C#で利用できる共有ライブラリを生成する設定です。

**--src/lib.rs のコード--**

```rust
#[no_mangle]
pub extern "C" fn hello_from_rust() {
    println!("Hello, Rust!");
}
```

## 2. Rustのビルド

Rustのプロジェクトをビルドして共有ライブラリを生成します。

```bash
cd rust_app
cargo build --release
```

このコマンドを実行すると、`target/release` ディレクトリに `librust_app.dylib`（macOSの場合）が生成されます。

## 3. RustのライブラリをC#プロジェクトにコピー

生成された `librust_app.dylib` をC#プロジェクトのディレクトリにコピーします。

```bash
cp target/release/librust_app.dylib ../c_sharp_app/
```

## 4. C#プロジェクトでRustライブラリを呼び出す

**--C#コードの設定--**

C#コードでは、`DllImport` を使ってRustの関数を呼び出します。

```csharp
using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("librust_app.dylib", CallingConvention = CallingConvention.Cdecl)]
    public static extern void hello_from_rust();

    static void Main()
    {
        hello_from_rust();
        Console.WriteLine("Hello from C#!");
    }
}
```

## 5. C#プロジェクトのビルドと実行

C#プロジェクトでクリーンビルドを行い、アプリケーションを実行します。

**--クリーンビルドと実行--**

```bash
cd c_sharp_app
dotnet clean
dotnet build
dotnet run
```

**--期待される出力--**

```bash
Hello, Rust!
Hello from C#!
```

---
