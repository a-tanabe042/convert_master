using System;
using System.IO;
using Conversions;

namespace Execute
{
    /// <summary>
    /// CSVファイルをSQL形式のファイルに変換するためのクラスです。
    /// </summary>
    /// <remarks>
    /// 指定されたCSVファイルを読み込み、SQLのINSERT文に変換します。
    /// 出力されたSQLファイルは、指定されたディレクトリに保存されます。
    /// </remarks>
    public static class CsvToSqlConverter
    {
        /// <summary>
        /// CSVファイルの変換処理を実行します。
        /// </summary>
        public static void Run()
        {
            try
            {
                // 実行ファイルのベースパスを取得
                string basePath = AppContext.BaseDirectory;

                // assets/csv フォルダのパスを設定
                string csvFolderPath = Path.Combine(basePath, "assets", "csv");

                // SQLファイルの出力先ディレクトリを設定
                string outputDirSql = Path.Combine(basePath, "data_output", "sql_output");

                // 出力ディレクトリが存在しない場合は作成
                if (!Directory.Exists(outputDirSql))
                    Directory.CreateDirectory(outputDirSql);

                // CSVファイルを取得（*.csv 形式のファイルのみ）
                string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

                // CSVファイルが見つからなかった場合のエラーメッセージ
                if (csvFiles.Length == 0)
                {
                    Console.WriteLine($"CSVファイルが見つかりません。パス: {csvFolderPath}");
                    return;
                }

                // 各CSVファイルを処理
                foreach (string csvFile in csvFiles)
                {
                    // ファイル名（拡張子なし）をテーブル名として取得
                    string tableName = Path.GetFileNameWithoutExtension(csvFile);
                    Console.WriteLine($"テーブル名: {tableName}");
                    Console.WriteLine($"処理中のCSVファイル: {csvFile}");

                    // CSVファイルをSQLクエリに変換して出力ディレクトリに保存
                    CsvToQueryConverter.Convert(csvFile, tableName, outputDirSql);
                }

                // すべての変換が完了したことを通知
                Console.WriteLine("全ての変換が完了しました。");
            }
            catch (Exception ex)
            {
                // エラーが発生した場合のメッセージ
                Console.WriteLine($"予期しないエラーが発生しました: {ex.Message}");
            }
        }
    }
}