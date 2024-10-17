using System;
using System.IO;
using Conversions;

namespace Execute
{
    public static class CsvToSqlConverter
    {
        public static void Run()
        {
            try
            {
                string basePath = AppContext.BaseDirectory;
                string csvFolderPath = Path.Combine(basePath, "assets", "csv");
                string outputDirSql = Path.Combine(basePath, "data_output", "sql_output");

                if (!Directory.Exists(outputDirSql))
                    Directory.CreateDirectory(outputDirSql);

                string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

                if (csvFiles.Length == 0)
                {
                    Console.WriteLine($"CSVファイルが見つかりません。パス: {csvFolderPath}");
                    return;
                }

                foreach (string csvFile in csvFiles)
                {
                    string tableName = Path.GetFileNameWithoutExtension(csvFile);
                    Console.WriteLine($"テーブル名: {tableName}");
                    Console.WriteLine($"処理中のCSVファイル: {csvFile}");

                    CsvToQueryConverter.Convert(csvFile, tableName, outputDirSql);
                }

                Console.WriteLine("全ての変換が完了しました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"予期しないエラーが発生しました: {ex.Message}");
            }
        }
    }
}