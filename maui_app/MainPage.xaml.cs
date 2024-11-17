using System;
using System.IO;
using System.Text;
using Microsoft.Maui.Controls;
using Foundation;
using UIKit;

namespace maui_app
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // 選択されたファイルの内容を保存するプロパティ
        private string? SelectedFileContent { get; set; }

        private async void OnSelectFolderClicked(object sender, EventArgs e)
        {
            try
            {
                var allowedUTIs = new string[] { "public.comma-separated-values-text" }; // CSVファイルのUTI
                var documentPicker = new UIDocumentPickerViewController(allowedUTIs, UIDocumentPickerMode.Import);
                documentPicker.Delegate = new FilePickerDelegate(this); // デリゲートを設定
                documentPicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                var viewController = Platform.GetCurrentUIViewController();
                if (viewController != null)
                {
                    await viewController.PresentViewControllerAsync(documentPicker, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
            }
        }

        private async void OnConvertAndDownloadClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedFileContent))
            {
                Console.WriteLine("ファイルが選択されていません。");
                return;
            }

            try
            {
                // CSV → JSON 変換（CsvToJsonConverterは事前に実装済みのクラスを使用）
                var jsonData = CsvToJsonConverter.Convert(SelectedFileContent);

                // JSONデータをバイト配列に変換
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

                // 一時ファイルを作成
                string tempFileName = $"output_{Guid.NewGuid()}.json";
                string tempFilePath = Path.Combine(FileSystem.CacheDirectory, tempFileName);
                await File.WriteAllBytesAsync(tempFilePath, jsonBytes);

                var fileUrl = new NSUrl(tempFilePath, false);

                // ダウンロード用の UIDocumentPickerViewController
                var documentPicker = new UIDocumentPickerViewController(new[] { fileUrl }, UIDocumentPickerMode.ExportToService);
                documentPicker.Delegate = new FilePickerDelegate(this); // デリゲートを設定
                documentPicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                var viewController = Platform.GetCurrentUIViewController();
                if (viewController != null)
                {
                    await viewController.PresentViewControllerAsync(documentPicker, true);
                }

                Console.WriteLine("変換が完了しました! ファイルをダウンロードしました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
            }
        }

        // 内部クラスとして FilePickerDelegate を定義
        private class FilePickerDelegate : UIDocumentPickerDelegate
        {
            private readonly MainPage mainPage;

            public FilePickerDelegate(MainPage mainPage)
            {
                this.mainPage = mainPage;
            }

            public override void WasCancelled(UIDocumentPickerViewController controller)
            {
                Console.WriteLine("ファイル選択がキャンセルされました。");
            }

            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
            {
                if (urls != null && urls.Length > 0)
                {
                    try
                    {
                        // NSUrlを使ってファイルの内容を読み込む
                        var fileUrl = urls[0];
                        var filePath = fileUrl.Path ?? string.Empty;

                        if(filePath == string.Empty)
                        {
                            Console.WriteLine("ファイルパスが空です。");
                            return;
                        }

                        mainPage.SelectedFileContent = File.ReadAllText(filePath);
                        fileUrl.StopAccessingSecurityScopedResource();
                        Console.WriteLine("ファイルの内容を取得しました。");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ファイルの読み込み中にエラーが発生しました: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("ファイル選択がキャンセルされました。");
                }
            }
        }
    }
}