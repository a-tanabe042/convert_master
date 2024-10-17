using System;
using System.IO;

namespace Conversions
{
    /// <summary>
    /// CSV → SQLクエリ変換クラス
    /// </summary>
    public static class CsvToQueryConverter
    {
        public static void Convert(string csvFolderPath, string tableName, string outputDirSql)
        {
            // tableNameをログ出力
            Console.WriteLine($"テーブル名: {tableName}");

            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

            foreach (string csvFilePath in csvFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
                string outputSqlPath = Path.Combine(outputDirSql, fileNameWithoutExtension + ".sql");

                Console.WriteLine($"CSVファイル {csvFilePath} を処理中...");

                try
                {
                    string csvData = File.ReadAllText(csvFilePath);
                    string queryData = QueryGenerator.GenerateInsertQuery(csvData, tableName);

                    File.WriteAllText(outputSqlPath, queryData);
                    Console.WriteLine($"SQLデータを {outputSqlPath} に保存しました");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"エラー: {csvFilePath} の処理中に問題が発生しました - {ex.Message}");
                }
            }
        }
    }
}
