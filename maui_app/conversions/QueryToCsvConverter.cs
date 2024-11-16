using System;
using System.IO;
using Interop;

namespace Conversions
{
    /// <summary>
    /// SQLクエリ → CSV変換クラス
    /// </summary>
    public static class QueryToCsvConverter
    {
        public static void Convert(string queryFolderPath, string outputDirCsv)
        {
            // 指定されたフォルダ内のすべてのSQLファイルを取得
            string[] queryFiles = Directory.GetFiles(queryFolderPath, "*.sql");
            foreach (string queryFilePath in queryFiles)
            {
                // ファイル名の拡張子を除いた部分を取得
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(queryFilePath);
                // 出力先のCSVファイルパスを作成
                string outputCsvPath = Path.Combine(outputDirCsv, fileNameWithoutExtension + ".csv");
                // SQLデータを読み込む
                string queryData = File.ReadAllText(queryFilePath);

                Console.WriteLine("QUERY → CSV 変換を実行します...");

                // Rust関数を呼び出して、SQLクエリをCSV形式に変換
                IntPtr csvPointer = RustFunctions.ConvertQueryToCsv(queryData);

                // 変換が失敗した場合は次のファイルへ
                if (csvPointer == IntPtr.Zero) continue;

                // ポインタから文字列に変換
                string csvData = RustFunctions.ConvertPointerToString(csvPointer);
                // CSVデータをファイルに書き込む
                File.WriteAllText(outputCsvPath, csvData);
                Console.WriteLine($"CSVデータを {outputCsvPath} に保存しました");

                // Rust側で確保したメモリを解放
                RustFunctions.FreeRustString(csvPointer);
            }
        }
    }
}