using System;
using System.Diagnostics;
using System.IO;
using Conversions;

class Program
{
    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            // bin/Debug/net8.0/以下にファイルがある場合は、そのパスを取得
            string basePath = AppContext.BaseDirectory;

            // assets配下のフォルダのパス
            string csvFolderPath = Path.Combine(basePath, "assets", "csv");
            string jsonFolderPath = Path.Combine(basePath, "assets", "json");
            string queryFolderPath = Path.Combine(basePath, "assets", "query");

            // 変換後のデータを保存するディレクトリ
            string outputDirCsv = Path.Combine(basePath, "data_output", "csv_output");
            string outputDirJson = Path.Combine(basePath, "data_output", "json_output");
            string outputDirSql = Path.Combine(basePath, "data_output", "query_output");

            // 出力ディレクトリが存在しない場合は作成
            if (!Directory.Exists(outputDirCsv)) Directory.CreateDirectory(outputDirCsv);
            if (!Directory.Exists(outputDirJson)) Directory.CreateDirectory(outputDirJson);
            if (!Directory.Exists(outputDirSql)) Directory.CreateDirectory(outputDirSql);

            // CSV → JSON 変換
            CsvToJsonConverter.Convert(csvFolderPath, outputDirJson);

            // CSV → QUERY 変換
            CsvToQueryConverter.Convert(csvFolderPath, "SampleTbl", outputDirSql);

            // JSON → CSV 変換
            JsonToCsvConverter.Convert(jsonFolderPath, outputDirCsv);

            // JSON → QUERY 変換
            JsonToQueryConverter.Convert(jsonFolderPath, outputDirSql);

            // QUERY → CSV 変換
            QueryToCsvConverter.Convert(queryFolderPath, outputDirCsv);

            // // JSON → QUERY 変換
            QueryToJsonConverter.Convert(queryFolderPath, outputDirJson);

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
            stopwatch.Stop();
            Console.WriteLine($"処理時間: {stopwatch.Elapsed.TotalSeconds} 秒");
        }
    }
}