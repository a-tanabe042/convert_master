using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Conversions;

namespace Execute
{
    /// <summary>
    /// NewTableAのマッピング処理を実行するクラス。
    /// 4つのCSVファイルからデータを読み込み、特定のカラムをJOINしてSQLクエリに変換し保存します。
    /// </summary>
    public static class NewTableAProcessor
    {
        /// <summary>
        /// NewTableAのマッピング処理を実行するメソッド。
        /// </summary>
        public static void Run()
        {
            try
            {
                // 実行ファイルのベースパスを取得。
                // AppContext.BaseDirectoryは、アプリケーションの実行パスを示します。
                string basePath = AppContext.BaseDirectory;

                // CSVファイルが格納されているフォルダのパスを構築。
                string csvFolderPath = Path.Combine(basePath, "assets", "csv");

                // SQLファイルの出力先ディレクトリのパスを構築。
                string outputDirSql = Path.Combine(basePath, "data_output", "sql_output");

                // SQLファイル出力ディレクトリが存在しない場合は作成。
                if (!Directory.Exists(outputDirSql))
                    Directory.CreateDirectory(outputDirSql);

                // 各CSVファイルを読み込む。
                var tableA = LoadCsv(Path.Combine(csvFolderPath, "test_table_a.csv"));
                var tableB = LoadCsv(Path.Combine(csvFolderPath, "test_table_b.csv"));
                var tableC = LoadCsv(Path.Combine(csvFolderPath, "test_table_c.csv"));
                var tableD = LoadCsv(Path.Combine(csvFolderPath, "test_table_d.csv"));

                // LINQでテーブルA, B, C, DをidでJOINし、必要なカラムをSELECT。
                var result = from a in tableA
                             join b in tableB on a["id"] equals b["id"]
                             join c in tableC on a["id"] equals c["id"]
                             join d in tableD on a["id"] equals d["id"]
                             select new Dictionary<string, string>
                             {
                                 { "FullName", a["name"] },  // 名前のカラム
                                 { "Email", b["email"] },    // メールアドレスのカラム
                                 { "BirthYear", (DateTime.Now.Year - int.Parse(c["age"])).ToString() },  // 生年を計算
                                 { "Department", d["department"] }  // 部署のカラム
                             };

                // 変換された結果をSQLファイルに保存。
                LinqToQueryConverter.Convert(result.ToList(), "new_table_a", outputDirSql);

                // 処理完了メッセージを出力。
                Console.WriteLine("NewTableAのマッピング結果をSQLファイルに保存しました。");
            }
            catch (Exception ex)
            {
                // エラーが発生した場合、その内容を表示。
                Console.WriteLine($"NewTableAの処理中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定されたCSVファイルを読み込み、辞書形式のリストに変換します。
        /// </summary>
        /// <param name="path">CSVファイルのパス。</param>
        /// <returns>CSVデータの辞書形式のリスト。</returns>
        private static List<Dictionary<string, string>> LoadCsv(string path)
        {
            // ファイルが存在しない場合はエラーを投げる。
            if (!File.Exists(path))
                throw new FileNotFoundException($"CSVファイルが見つかりません: {path}");

            // ファイルの全行を読み込む。
            var lines = File.ReadAllLines(path);

            // ヘッダー行（1行目）を取得し、カラム名として使用。
            var headers = lines[0].Split(',');

            // データ行を辞書形式に変換し、リストとして返す。
            return lines.Skip(1)  // 1行目（ヘッダー）はスキップ。
                        .Select(line => line.Split(','))  // 各行をカンマで分割。
                        .Select(values => headers.Zip(values, (header, value) => new { header, value })
                                                 .ToDictionary(x => x.header, x => x.value))  // ヘッダーと値を辞書に変換。
                        .ToList();  // リストとして返す。
        }
    }
}