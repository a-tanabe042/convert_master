using FileConverter;

namespace Conversions
{
    /// <summary>
    /// SQLクエリ → CSV変換クラス
    /// </summary>
    public static class QueryToCsvConverter
    {
        public static string Convert(string queryData)
        {
            // 開始ログ
            Console.WriteLine("QUERY → CSV 変換を実行します...");

            // ポインタ取得
            IntPtr csvPointer = DataManager.ConvertQueryToCsv(queryData);

            // ポインタを文字列に変換
            string csvData = DataManager.ConvertPointerToString(csvPointer);

            // Rust側で確保したメモリを解放
            //DataManager.FreeRustString(csvPointer);

            return csvData;
        }
    }
}