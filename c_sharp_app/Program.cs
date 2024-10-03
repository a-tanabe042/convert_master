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
            string jsonFolderPath = Path.Combine(basePath, "assets", "json");

            // 出力ディレクトリ
            string outputDirCsv = Path.Combine(basePath, "data_output", "csv_output");
            string outputDirJson = Path.Combine(basePath, "data_output", "json_output");

            // 出力ディレクトリが存在しない場合は作成
            if (!Directory.Exists(outputDirCsv))
            {
                Directory.CreateDirectory(outputDirCsv);
            }
            if (!Directory.Exists(outputDirJson))
            {
                Directory.CreateDirectory(outputDirJson);
            }

            // CSV → JSON 変換
            ConvertCsvToJson(csvFolderPath, outputDirJson);

            // JSON → CSV 変換
            ConvertJsonToCsv(jsonFolderPath, outputDirCsv);

            // CSV  → QUERY 変換
            ConvertCsvToQuery(csvFolderPath, "SampleTbl", outputDirJson);

            Console.WriteLine("全ての変換が完了しました。");
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

    static void ConvertCsvToJson(string csvFolderPath, string outputDirJson)
    {
        string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");
        foreach (string csvFilePath in csvFiles)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
            string outputJsonPath = Path.Combine(outputDirJson, fileNameWithoutExtension + ".json");

            string csvData = File.ReadAllText(csvFilePath);

            Console.WriteLine("CSV → JSON 変換を実行します...");
            IntPtr jsonPointer = RustInterop.ConvertCsvToJson(csvData);

            if (jsonPointer == IntPtr.Zero)
            {
                continue;
            }

            string jsonData = RustInterop.ConvertPointerToString(jsonPointer);

            File.WriteAllText(outputJsonPath, jsonData);
            Console.WriteLine($"JSONデータを {outputJsonPath} に保存しました");
        }
    }

    static void ConvertJsonToCsv(string jsonFolderPath, string outputDirCsv)
    {
        string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");
        foreach (string jsonFilePath in jsonFiles)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonFilePath);
            string outputCsvPath = Path.Combine(outputDirCsv, fileNameWithoutExtension + ".csv");

            string jsonData = File.ReadAllText(jsonFilePath);

            Console.WriteLine("JSON → CSV 変換を実行します...");
            IntPtr csvPointer = RustInterop.ConvertJsonToCsv(jsonData);

            if (csvPointer == IntPtr.Zero)
            {
                continue;
            }

            string csvData = RustInterop.ConvertPointerToString(csvPointer);

            File.WriteAllText(outputCsvPath, csvData);
            Console.WriteLine($"CSVデータを {outputCsvPath} に保存しました");
        }
    }

    static void ConvertCsvToQuery(string csvFolderPath, string tableName, string outputDirSql)
    {
        string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

        foreach (string csvFilePath in csvFiles)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
            string outputSqlPath = Path.Combine(outputDirSql, fileNameWithoutExtension + ".sql");

            string csvData = File.ReadAllText(csvFilePath);

            Console.WriteLine("CSV → QUERY 変換を実行します...");
            IntPtr sqlPointer = RustInterop.ConvertCsvToQuery(csvData, tableName);

            if (sqlPointer == IntPtr.Zero)
            {
                continue;
            }

            string queryData = RustInterop.ConvertPointerToString(sqlPointer);

            File.WriteAllText(outputSqlPath, queryData);
            Console.WriteLine($"CSVデータを {outputSqlPath} に保存しました");
        }
    }
}

public static class RustInterop
{
    [DllImport("librust_app.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "csv_to_json")]
    public static extern IntPtr ConvertCsvToJson(string csvData);

    [DllImport("librust_app.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "json_to_csv")]
    public static extern IntPtr ConvertJsonToCsv(string jsonData);

    [DllImport("rust_app.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "csv_to_query")]
    public static extern IntPtr ConvertCsvToQuery(string csvData, string tableName);

    public static string ConvertPointerToString(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            return null;
        }
        return Marshal.PtrToStringAnsi(pointer);
    }
}