using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Conversions;

namespace Execute
{
    /// <summary>
    /// 複数のCSVデータをJOINし、選択したカラムを取得してSQLファイルに保存するクラス。
    /// </summary>
    public static class MappingProcessor
    {
        /// <summary>
        /// マッピング処理を実行します。
        /// </summary>
        public static void Run()
        {
            try
            {
                // 実行ファイルのベースパスを取得
                string basePath = AppContext.BaseDirectory;

                // assets/csv フォルダのパスを設定
                string csvFolderPath = Path.Combine(basePath, "assets", "csv");

                // SQLファイルの出力先ディレクトリを設定
                string outputDirSql = Path.Combine(basePath, "data_output", "sql_output");

                // 出力ディレクトリが存在しない場合は作成
                if (!Directory.Exists(outputDirSql))
                    Directory.CreateDirectory(outputDirSql);

                // CSVファイルを読み込む
                var tableA = LoadCsv(Path.Combine(csvFolderPath, "test_table_a.csv"));
                var tableB = LoadCsv(Path.Combine(csvFolderPath, "test_table_b.csv"));
                var tableC = LoadCsv(Path.Combine(csvFolderPath, "test_table_c.csv"));
                var tableD = LoadCsv(Path.Combine(csvFolderPath, "test_table_d.csv"));

                // LINQでテーブルA, B, C, DをJOINし、必要なカラムだけをSELECT
                var result = from a in tableA
                             join b in tableB on a["id"] equals b["id"]
                             join c in tableC on a["id"] equals c["id"]
                             join d in tableD on a["id"] equals d["id"]
                             select new Dictionary<string, string>
                             {
                                 { "FullName", a["name"] },
                                 { "Email", b["email"] },
                                 { "BirthYear", (DateTime.Now.Year - int.Parse(c["age"])).ToString() },
                                 { "Department", d["department"] }
                             };

                // LINQ結果をSQLファイルに変換して保存
                LinqToQueryConverter.Convert(result.ToList(), "mapped_data", outputDirSql);

                Console.WriteLine("マッピング結果をSQLファイルに保存しました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// CSVファイルを読み込んで辞書形式のリストに変換するメソッド。
        /// </summary>
        /// <param name="path">CSVファイルのパス</param>
        /// <returns>CSVデータの辞書形式のリスト</returns>
        private static List<Dictionary<string, string>> LoadCsv(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"CSVファイルが見つかりません: {path}");
            }

            var lines = File.ReadAllLines(path);
            var headers = lines[0].Split(',');

            return lines.Skip(1)
                        .Select(line => line.Split(','))
                        .Select(values => headers.Zip(values, (header, value) => new { header, value })
                                                 .ToDictionary(x => x.header, x => x.value))
                        .ToList();
        }
    }
}