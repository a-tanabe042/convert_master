
## コマンド

### Rustプロジェクトのビルドと実行

```bash
cargo new rust_app --lib

cd rust_app
cargo build --release
cargo run
```

### C#プロジェクトのビルドと実行

```bash
dotnet new console -o c_sharp_app

cd c_sharp_app
dotnet build
dotnet run
```

mac os 
dotnet clean
dotnet build --framework net8.0-maccatalyst
dotnet run --framework net8.0-maccatalyst