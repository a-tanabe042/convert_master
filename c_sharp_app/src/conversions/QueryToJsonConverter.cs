using System;
using System.IO;
using Interop;

namespace Conversions
{
    /// <summary>
    /// SQLクエリ → JSON変換クラス
    /// </summary>
    public static class QueryToJsonConverter
    {
        public static void Convert(string queryFolderPath, string outputDirJson)
        {
            // 指定されたフォルダ内のすべてのSQLファイルを取得
            string[] queryFiles = Directory.GetFiles(queryFolderPath, "*.sql");
            foreach (string queryFilePath in queryFiles)
            {
                // ファイル名の拡張子を除いた部分を取得
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(queryFilePath);
                // 出力先のJSONファイルパスを作成
                string outputJsonPath = Path.Combine(outputDirJson, fileNameWithoutExtension + ".json");
                // SQLデータを読み込む
                string queryData = File.ReadAllText(queryFilePath);

                Console.WriteLine("QUERY → JSON 変換を実行します...");

                // Rust関数を呼び出して、SQLクエリをJSON形式に変換
                IntPtr jsonPointer = RustFunctions.ConvertQueryToJson(queryData);

                // 変換が失敗した場合は次のファイルへ
                if (jsonPointer == IntPtr.Zero) continue;

                // ポインタから文字列に変換し、メモリを解放
                string jsonData = RustFunctions.ConvertPointerToString(jsonPointer);

                // JSONデータをファイルに書き込む
                File.WriteAllText(outputJsonPath, jsonData);
                Console.WriteLine($"JSONデータを {outputJsonPath} に保存しました");

                // Rust側で確保したメモリを解放
                RustFunctions.FreeRustString(jsonPointer);
            }
        }
    }
}