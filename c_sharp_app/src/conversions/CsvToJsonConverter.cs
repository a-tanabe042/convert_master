using System;
using System.IO;
using Interop;

namespace Conversions
{
    /// <summary>
    /// CSV → JSON 変換クラス
    /// </summary>
    public static class CsvToJsonConverter
    {
        public static void Convert(string csvFolderPath, string outputDirJson)
        {
            // 指定されたフォルダ内のすべてのCSVファイルを取得
            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");
            foreach (string csvFilePath in csvFiles)
            {
                // ファイル名の拡張子を除いた部分を取得
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
                // 出力先のJSONファイルパスを作成
                string outputJsonPath = Path.Combine(outputDirJson, fileNameWithoutExtension + ".json");
                // CSVデータを読み込む
                string csvData = File.ReadAllText(csvFilePath);

                Console.WriteLine("CSV → JSON 変換を実行します...");

                // Rust関数を呼び出して、CSVをJSONに変換
                IntPtr jsonPointer = RustFunctions.ConvertCsvToJson(csvData);

                // 変換が失敗した場合は次のファイルへ
                if (jsonPointer == IntPtr.Zero) continue;

                // ポインタから文字列に変換
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