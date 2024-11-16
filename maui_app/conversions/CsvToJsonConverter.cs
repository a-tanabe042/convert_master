using System;
using System.IO;
using System.Collections.Generic;
using Interop;

namespace maui_app
{
    /// <summary>
    /// CSV → JSON 変換クラス
    /// </summary>
    public static class CsvToJsonConverter
    {
        public static string Convert(string csvFolderPath)
        {            
            string jsonData = string.Empty;

            // 指定されたフォルダ内のすべてのCSVファイルを取得
            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");
            foreach (string csvFilePath in csvFiles)
            {
                // CSVデータを読み込む
                string csvData = File.ReadAllText(csvFilePath);
                Console.WriteLine("CSV → JSON 変換を実行します...");

                // Rust関数を呼び出して、CSVをJSONに変換
                IntPtr jsonPointer = RustFunctions.ConvertCsvToJson(csvData);

                // 変換が失敗した場合は次のファイルへ
                if (jsonPointer == IntPtr.Zero) continue;

                // ポインタから文字列に変換
                jsonData = RustFunctions.ConvertPointerToString(jsonPointer);

                Console.WriteLine($"JSONデータを作成しました: {csvFilePath}");

                // Rust側で確保したメモリを解放
                RustFunctions.FreeRustString(jsonPointer);
            }

            return jsonData;
        }
    }
}