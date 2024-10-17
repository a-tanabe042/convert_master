using System;
using Execute;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("エラー: コマンドが指定されていません。");
            Console.WriteLine("使用例: dotnet run convert または dotnet run mapping");
            return;
        }

        string command = args[0].ToLower();

        switch (command)
        {
            case "convert":
                // csvファイルをsqlファイルに変換
                CsvToSqlConverter.Run();
                break;

            case "mapping":
                // 旧データから新データへのマッピング処理を実行
                MappingProcessor.Run();
                break;

            default:
                Console.WriteLine($"エラー: 未知のコマンド '{command}' が指定されました。");
                Console.WriteLine("サポートされているコマンド: convert, mapping");
                break;
        }
    }
}