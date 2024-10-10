using System;
using System.IO;
using Interop;

namespace Conversions
{
    // <summary>
    // CSV → SQLクエリ変換クラス
    // </summary>
    public static class CsvToQueryConverter
    {
        public static void Convert(string csvFolderPath, string tableName, string outputDirSql)
        {
            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");
            foreach (string csvFilePath in csvFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
                string outputSqlPath = Path.Combine(outputDirSql, fileNameWithoutExtension + ".sql");
                string csvData = File.ReadAllText(csvFilePath);

                Console.WriteLine("CSV → QUERY 変換を実行します...");

                IntPtr sqlPointer = RustFunctions.ConvertCsvToQuery(csvData, tableName);

                if (sqlPointer == IntPtr.Zero) continue;

                string queryData = RustFunctions.ConvertPointerToString(sqlPointer);
                File.WriteAllText(outputSqlPath, queryData);
                Console.WriteLine($"SQLデータを {outputSqlPath} に保存しました");
            }
        }
    }
}