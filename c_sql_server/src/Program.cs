using System;
using System.IO;
using Conversions;

class Program
{
    static void Main()
    {
        try
        {
            // 実行ファイルのベースパスを取得
            string basePath = AppContext.BaseDirectory;

            // assets/csv フォルダのパス
            string csvFolderPath = Path.Combine(basePath, "assets", "csv");

            // SQL出力ディレクトリのパス
            string outputDirSql = Path.Combine(basePath, "data_output", "sql_output");

            // ディレクトリの存在を確認、なければ作成
            if (!Directory.Exists(outputDirSql))
                Directory.CreateDirectory(outputDirSql);

            // CSVファイルの取得
            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

            if (csvFiles.Length == 0)
            {
                Console.WriteLine($"CSVファイルが見つかりません。パス: {csvFolderPath}");
                return;
            }

            // 各CSVファイルを処理
            foreach (string csvFile in csvFiles)
            {
                // ファイル名からテーブル名を取得（拡張子なし）
                string tableName = Path.GetFileNameWithoutExtension(csvFile);

                Console.WriteLine($"テーブル名: {tableName}");
                Console.WriteLine($"処理中のCSVファイル: {csvFile}");

                // CSV → QUERY 変換の実行
                CsvToQueryConverter.Convert(csvFile, tableName, outputDirSql);
            }

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
            Console.WriteLine("アプリケーションを終了します。");
        }
    }
}