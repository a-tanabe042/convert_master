using System;
using System.Linq;

namespace Conversions
{
    /// <summary>
    /// SQLクエリ生成クラス
    /// </summary>
    public static class QueryGenerator
    {
        /// <summary>
        /// CSVデータを基にSQL INSERTクエリを生成します。
        /// </summary>
        /// <param name="csvData">CSV形式のデータ</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>生成されたSQL INSERTクエリ文字列</returns>
        public static string GenerateInsertQuery(string csvData, string tableName)
        {
            // 各行を分割し、空白行を除外
            var lines = csvData.Split(Environment.NewLine)
                               .Where(line => !string.IsNullOrWhiteSpace(line))
                               .ToArray();

            // ヘッダー行がない場合はエラーを投げる
            if (lines.Length < 2)
                throw new InvalidOperationException("CSVファイルのフォーマットが正しくありません。");

            // 1行目をヘッダーとして取得
            var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();

            // データ行を加工し、SQL用の形式に変換
            var valueLines = lines.Skip(1)
                                  .Select(line => ConvertLineToSqlValues(line));

            // SQLクエリを構築
            string query = $"INSERT INTO {tableName} ({string.Join(", ", headers)})\nVALUES\n{string.Join(",\n", valueLines)};";
            return query;
        }

        /// <summary>
        /// CSVの1行をSQL形式の値に変換します。
        /// </summary>
        /// <param name="line">CSVの1行</param>
        /// <returns>SQL形式の値</returns>
        private static string ConvertLineToSqlValues(string line)
        {
            var values = line.Split(',')
                             .Select(value => ConvertValue(value.Trim()));
            return $"({string.Join(", ", values)})";
        }

        /// <summary>
        /// 値を適切なSQL形式に変換します。
        /// 数値はそのまま、文字列はシングルクォートで囲みます。
        /// </summary>
        /// <param name="value">CSVのセルの値</param>
        /// <returns>SQL用の文字列</returns>
        private static string ConvertValue(string value)
        {
            // 数値かどうかを判定し、数値の場合はそのまま返す
            if (int.TryParse(value, out _))
            {
                return value;
            }
            // それ以外はシングルクォートで囲む
            return $"'{value}'";
        }
    }
}