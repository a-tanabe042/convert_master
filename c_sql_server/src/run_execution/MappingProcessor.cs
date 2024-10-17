using System;

namespace Execute
{
    /// <summary>
    /// 各NewTableの処理をまとめて実行するクラス
    /// </summary>
    public static class MappingProcessor
    {
        /// <summary>
        /// 全てのNewTableのマッピング処理を実行します。
        /// </summary>
        public static void RunAll()
        {
            try
            {
                Console.WriteLine("全てのNewTableのマッピング処理を開始します。");

                // テーブルAの処理を実行
                NewTableAProcessor.Run();
                // テーブルBの処理を実行
                NewTableBProcessor.Run();
                // テーブルCの処理を実行
                NewTableCProcessor.Run();
                // テーブルDの処理を実行
                NewTableDProcessor.Run();

                Console.WriteLine("全てのNewTableのマッピング処理が完了しました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }
    }
}