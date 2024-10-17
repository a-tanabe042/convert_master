using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Conversions
{
    /// <summary>
    /// LINQの結果をSQL INSERTクエリに変換するクラス
    /// </summary>
    public static class LinqToQueryConverter
    {
        /// <summary>
        /// LINQ結果をSQL INSERTクエリに変換し、指定フォルダに保存します。
        /// </summary>
        /// <param name="data">LINQから取得したデータのリスト</param>
        /// <param name="tableName">SQLテーブル名</param>
        /// <param name="outputDirSql">SQLファイルの出力ディレクトリ</param>
        public static void Convert(List<Dictionary<string, string>> data, string tableName, string outputDirSql)
        {
            Console.WriteLine($"テーブル名: {tableName}");

            try
            {
                // 出力するSQLファイルのパスを構築
                string outputSqlPath = Path.Combine(outputDirSql, $"{tableName}.sql");

                Console.WriteLine($"SQLファイル {outputSqlPath} を生成中...");

                // SQLクエリを生成
                string queryData = GenerateInsertQuery(data, tableName);

                // SQLファイルへの書き込み
                File.WriteAllText(outputSqlPath, queryData);

                Console.WriteLine($"SQLデータを {outputSqlPath} に保存しました");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: SQLファイルの生成中に問題が発生しました - {ex.Message}");
            }
        }

        /// <summary>
        /// データをSQL INSERTクエリに変換します。
        /// </summary>
        /// <param name="data">変換するデータのリスト</param>
        /// <param name="tableName">SQLテーブル名</param>
        /// <returns>生成されたSQL INSERTクエリ</returns>
        private static string GenerateInsertQuery(List<Dictionary<string, string>> data, string tableName)
        {
            if (data == null || data.Count == 0)
                throw new InvalidOperationException("データが存在しません。");

            // カラム名を取得（データの1行目のキーを使用）
            var columns = data[0].Keys;
            string columnsJoined = string.Join(", ", columns);

            // 各行のデータをSQLの値形式に変換
            var valuesList = data.Select(row =>
                $"({string.Join(", ", row.Values.Select(value => $"'{EscapeSingleQuotes(value)}'"))})"
            );

            // INSERTクエリを生成
            string query = $"INSERT INTO {tableName} ({columnsJoined})\nVALUES\n{string.Join(",\n", valuesList)};";
            return query;
        }

        /// <summary>
        /// シングルクォートをエスケープします。
        /// </summary>
        /// <param name="value">エスケープする文字列</param>
        /// <returns>エスケープされた文字列</returns>
        private static string EscapeSingleQuotes(string value)
        {
            return value.Replace("'", "''");
        }
    }
}