namespace FileConverter
{
    /// <summary>
    /// CSV ⇒ SQLクエリ変換クラス
    /// </summary>
    public static class CsvToQueryConverter
    {
        /// <summary>
        /// CSV ⇒ SQLクエリ変換実行
        /// </summary>
        /// <param name="csvData">csvデータ（テキスト形式）</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>SQLクエリ</returns>
        public static string Convert(string csvData, string tableName)
        {
            // 開始ログ
            Console.WriteLine("CSV → QUERY 変換を実行します...");

            // ポインタ取得
            nint sqlPointer = DataManager.ConvertCsvToQuery(csvData, tableName);

            // ポインタを文字列に変換
            string queryData = DataManager.ConvertPointerToString(sqlPointer);

            // Rust側で確保したメモリを解放
            //DataManager.FreeRustString(sqlPointer);

            return queryData;
        }
    }
}