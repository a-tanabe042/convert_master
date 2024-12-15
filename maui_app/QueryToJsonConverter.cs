using FileConverter;

namespace Conversions
{
    /// <summary>
    /// SQLクエリ → JSON変換クラス
    /// </summary>
    public static class QueryToJsonConverter
    {
        public static string Convert(string queryData)
        {
            // 開始ログ
            Console.WriteLine("QUERY → JSON 変換を実行します...");

            // ポインタ取得
            IntPtr jsonPointer = DataManager.ConvertQueryToJson(queryData);

            // ポインタを文字列に変換
            string jsonData = DataManager.ConvertPointerToString(jsonPointer);

            // Rust側で確保したメモリを解放
            //DataManager.FreeRustString(jsonPointer);

            return jsonData;
        }
    }
}