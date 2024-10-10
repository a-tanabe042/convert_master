using System;
using System.IO;
using Interop;

namespace Conversions
{
    // <summary>
    // CSV → JSON 変換クラス
    // </summary>
    public static class CsvToJsonConverter
    {
        public static void Convert(string csvFolderPath, string outputDirJson)
        {
            string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");
            foreach (string csvFilePath in csvFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFilePath);
                string outputJsonPath = Path.Combine(outputDirJson, fileNameWithoutExtension + ".json");
                string csvData = File.ReadAllText(csvFilePath);

                Console.WriteLine("CSV → JSON 変換を実行します...");

                IntPtr jsonPointer = RustFunctions.ConvertCsvToJson(csvData);

                if (jsonPointer == IntPtr.Zero) continue;

                string jsonData = RustFunctions.ConvertPointerToString(jsonPointer);
                File.WriteAllText(outputJsonPath, jsonData);
                Console.WriteLine($"JSONデータを {outputJsonPath} に保存しました");
            }
        }
    }
}