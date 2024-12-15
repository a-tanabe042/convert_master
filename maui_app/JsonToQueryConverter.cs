using FileConverter;

namespace Conversions
{
    /// <summary>
    /// JSON → SQLクエリ変換クラス
    /// </summary>
    public static class JsonToQueryConverter
    {
        public static string Convert(string jsonData)
        {
            // 開始ログ
            Console.WriteLine("JSON → SQLクエリ変換を実行します...");

            // ポインタ取得
            IntPtr sqlPointer = DataManager.ConvertJsonToQuery(jsonData);

            // ポインタを文字列に変換
            string sqlData = DataManager.ConvertPointerToString(sqlPointer);

            // Rust側で確保したメモリを解放
            //DataManager.FreeRustString(sqlPointer);

            return sqlData;
        }
    }
}