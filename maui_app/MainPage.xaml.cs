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

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                CounterBtn.Text = $"Hello {CounterBtn.Text}";

                // CSV フォルダのパスを指定
                string csvFolderPath = "Contents/Resources/csv";

                // CSV → JSON 変換を実行
                var jsonData = CsvToJsonConverter.Convert(csvFolderPath);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

                // 一時ファイルを作成
                string tempFileName = $"output_{Guid.NewGuid()}.json";
                string tempFilePath = Path.Combine(FileSystem.CacheDirectory, tempFileName);
                await File.WriteAllBytesAsync(tempFilePath, jsonBytes);

                var fileUrl = new NSUrl(tempFilePath, false);

                // UIDocumentPickerViewController を使用してファイルをエクスポート
                var documentPicker = new UIDocumentPickerViewController(new[] { fileUrl }, UIDocumentPickerMode.ExportToService);
                documentPicker.Delegate = new DocumentPickerDelegate();
                documentPicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                var viewController = Platform.GetCurrentUIViewController();
                if (viewController != null)
                {
                    await viewController.PresentViewControllerAsync(documentPicker, true);
                }

                CounterBtn.Text = "変換が完了しました! ファイルを保存しました。";
            }
            catch (Exception ex)
            {
                CounterBtn.Text = $"エラー: {ex.Message}";
            }
        }
    }

    // UIDocumentPicker のデリゲートクラスを定義
    public class DocumentPickerDelegate : UIDocumentPickerDelegate
    {
        public override void WasCancelled(UIDocumentPickerViewController controller)
        {
            Console.WriteLine("ファイル選択がキャンセルされました。");
        }

        public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
        {
            if (urls != null && urls.Length > 0)
            {
                Console.WriteLine("ファイルが保存されました: " + urls[0].Path);
            }
            else
            {
                Console.WriteLine("ファイル選択がキャンセルされました。");
            }
        }
    }
}