using FileConverter;

namespace Conversions
{
    // <summary>
    // JSON → CSV 変換クラス
    // </summary>
    public static class JsonToCsvConverter
    {
        public static string Convert(string jsonData)
        {
            // 開始ログ
            Console.WriteLine("JSON → CSV 変換を実行します...");

            // ポインタ取得
            IntPtr csvPointer = DataManager.ConvertJsonToCsv(jsonData);

            // ポインタを文字列に変換
            string csvData = DataManager.ConvertPointerToString(csvPointer);

            // Rust側で確保したメモリを解放
            //DataManager.FreeRustString(csvPointer);

            return csvData;
        }
    }
}