namespace FileConverter
{
    /// <summary>
    /// CSV ⇒ JSON変換クラス
    /// </summary>
    public static class CsvToJsonConverter
    {
        /// <summary>
        /// CSV ⇒ JSON変換実行
        /// </summary>
        /// <param name="csvData">csvデータ（テキスト形式）</param>
        /// <returns>JSONデータ（テキスト）</returns>
        public static string Convert(string csvData)
        {
            // 開始ログ
            Console.WriteLine("CSV → JSON 変換を実行します...");

            // ポインタ取得
            IntPtr jsonPointer = DataManager.ConvertCsvToJson(csvData);

            // ポインタを文字列に変換
            string jsonData = DataManager.ConvertPointerToString(jsonPointer);

            // Rust側で確保したメモリを解放
            //DataManager.FreeRustString(jsonPointer);

            return jsonData;
        }
    }
}