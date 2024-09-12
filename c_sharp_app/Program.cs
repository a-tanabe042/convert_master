using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {
        // Stopwatchを使って実行時間を計測
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            // 実行ファイルがあるディレクトリを基準にする
            string basePath = AppContext.BaseDirectory;

            // assets/csvフォルダのパス
            string csvFolderPath = Path.Combine(basePath, "assets", "csv");

            // 出力ディレクトリ
            string outputDir = Path.Combine(basePath, "data_output");

            // 出力ディレクトリが存在しない場合は作成
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // CSVフォルダ内の全てのCSVファイルを処理
            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");
            foreach (string csvFilePath in csvFiles)
            {
                // ファイル名を取得し、拡張子を .json に変更
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
                string outputJsonPath = Path.Combine(outputDir, fileNameWithoutExtension + ".json");
                // CSVファイルを読み込む
                string csvData = File.ReadAllText(csvFilePath);

                // CSV → JSON 変換（Rustの関数を呼び出し）
                Console.WriteLine("CSV → JSON 変換を実行します...");
                IntPtr jsonPointer = RustInterop.ConvertCsvToJson(csvData);

                if (jsonPointer == IntPtr.Zero)
                {
                    continue;
                }
                // JSONデータをC#の文字列に変換
                string jsonData = RustInterop.ConvertPointerToString(jsonPointer);

                // JSONデータをファイルに保存
                File.WriteAllText(outputJsonPath, jsonData);
                Console.WriteLine($"JSONデータを {outputJsonPath} に保存しました");
            }

            Console.WriteLine("全てのCSVファイルの変換が完了しました。");
        }
        catch (DllNotFoundException ex)
        {
            Console.WriteLine($"エラー: ライブラリが見つかりません: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"予期しないエラーが発生しました: {ex.Message}");
        }
        finally
        {
            // Stopwatchを停止して、経過時間を表示
            stopwatch.Stop();
            Console.WriteLine($"処理時間: {stopwatch.Elapsed.TotalSeconds} 秒");
        }
    }
}

// Rustの関数を呼び出すためのインターフェース
public static class RustInterop
{
    // Rustで実装した CSV → JSON 変換関数をインポート
    [DllImport("librust_app.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "csv_to_json")]
    // これは、Rustの関数のシグネチャに合わせています
    public static extern IntPtr ConvertCsvToJson(string csvData);

    // 文字列ポインタをC#の文字列に変換するためのユーティリティ関数
    public static string ConvertPointerToString(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            return null;
        }
        // ポインタから文字列に変換
        return Marshal.PtrToStringAnsi(pointer);
    }
}
