using FileConverter;

namespace Conversions
{
    /// <summary>
    /// JSON → SQLクエリ変換クラス
    /// </summary>
    public static class JsonToQueryConverter
    {
        public static void Convert(string jsonFolderPath, string outputDirSql)
        {
            // 指定されたフォルダ内のすべてのJSONファイルを取得
            string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");
            foreach (string jsonFilePath in jsonFiles)
            {
                // ファイル名の拡張子を除いた部分を取得
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonFilePath);
                // 出力先のSQLファイルパスを作成
                string outputSqlPath = Path.Combine(outputDirSql, fileNameWithoutExtension + ".sql");
                // JSONデータを読み込む
                string jsonData = File.ReadAllText(jsonFilePath);

                Console.WriteLine("JSON → SQLクエリ変換を実行します...");

                // Rust関数を呼び出して、JSONをSQLクエリに変換
                IntPtr sqlPointer = DataManager.ConvertJsonToQuery(jsonData);

                // 変換が失敗した場合は次のファイルへ
                if (sqlPointer == IntPtr.Zero) continue;

                // ポインタから文字列に変換
                string sqlData = DataManager.ConvertPointerToString(sqlPointer);
                // SQLデータをファイルに書き込む
                File.WriteAllText(outputSqlPath, sqlData);
                Console.WriteLine($"SQLクエリを {outputSqlPath} に保存しました");

                // Rust側で確保したメモリを解放
                DataManager.FreeRustString(sqlPointer);
            }
        }
    }
}