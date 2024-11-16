using System;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Conversions;

namespace maui_app
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                CounterBtn.Text = $"Hello {CounterBtn.Text}";

                // CSV フォルダのパスを指定
                string csvFolderPath = "Contents/Resources/csv";

                // ユーザーのドキュメントフォルダを取得
                string userDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // ファイル名と保存先パスを指定
                string fileName = $"output_{Guid.NewGuid()}.json";
                string filePath = Path.Combine(userDocumentsPath, fileName);

                // CSV → JSON 変換を実行
                var jsonData = CsvToJsonConverter.Convert(csvFolderPath);

                // JSON データをユーザーのドキュメントフォルダに保存
                await File.WriteAllTextAsync(filePath, jsonData);

                CounterBtn.Text = $"変換が完了しました! ファイルが {filePath} に保存されました。";
            }
            catch (Exception ex)
            {
                CounterBtn.Text = $"エラー: {ex.Message}";
            }
        }
    }
}