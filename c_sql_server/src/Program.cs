using System;
using System.IO;
using Conversions;

class Program
{
    static void Main()
    {
        try
        {
            // bin/Debug/net8.0/以下にファイルがある場合は、そのパスを取得
            string basePath = AppContext.BaseDirectory;

            // assets配下のフォルダのパス
            string csvFolderPath = Path.Combine(basePath, "assets", "csv");

            // 変換後のデータを保存するディレクトリ
            string outputDirSql = Path.Combine(basePath, "data_output", "sql_output");

            // 出力ディレクトリが存在しない場合は作成
            if (!Directory.Exists(outputDirSql)) Directory.CreateDirectory(outputDirSql);

            // CSV → QUERY 変換
            CsvToQueryConverter.Convert(csvFolderPath, "SampleTbl", outputDirSql);

            Console.WriteLine("全ての変換が完了しました。");
        }
        catch (DllNotFoundException ex)
        {
            Console.WriteLine($"エラー: ライブラリが見つかりません: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"予期しないエラーが発生しました: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("アプリケーションを終了します。");
        }
    }
}