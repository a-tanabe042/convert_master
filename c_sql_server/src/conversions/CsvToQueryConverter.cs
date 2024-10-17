using System;
using System.IO;

namespace Conversions
{
    /// <summary>
    /// CSV → SQLクエリ変換クラス
    /// </summary>
    public static class CsvToQueryConverter
    {
        public static void Convert(string csvFilePath, string tableName, string outputDirSql)
        {
            // テーブル名のログ出力
            Console.WriteLine($"テーブル名: {tableName}");

            try
            {
                // 出力するSQLファイルのパスを構築
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
                string outputSqlPath = Path.Combine(outputDirSql, fileNameWithoutExtension + ".sql");

                Console.WriteLine($"CSVファイル {csvFilePath} を処理中...");

                // CSVファイルの読み込み
                string csvData = File.ReadAllText(csvFilePath);

                // SQLクエリの生成
                string queryData = QueryGenerator.GenerateInsertQuery(csvData, tableName);

                // SQLファイルへの書き込み
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