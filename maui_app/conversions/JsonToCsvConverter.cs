using System;
using System.IO;
using Interop;

namespace Conversions
{
    // <summary>
    // JSON → CSV 変換クラス
    // </summary>
    public static class JsonToCsvConverter
    {
        public static void Convert(string jsonFolderPath, string outputDirCsv)
        {
            string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");
            foreach (string jsonFilePath in jsonFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonFilePath);
                string outputCsvPath = Path.Combine(outputDirCsv, fileNameWithoutExtension + ".csv");
                string jsonData = File.ReadAllText(jsonFilePath);

                Console.WriteLine("JSON → CSV 変換を実行します...");

                IntPtr csvPointer = RustFunctions.ConvertJsonToCsv(jsonData);

                if (csvPointer == IntPtr.Zero) continue;

                string csvData = RustFunctions.ConvertPointerToString(csvPointer);
                File.WriteAllText(outputCsvPath, csvData);
                Console.WriteLine($"CSVデータを {outputCsvPath} に保存しました");

                // Rust側で確保したメモリを解放
                RustFunctions.FreeRustString(csvPointer);
            }
        }
    }
}