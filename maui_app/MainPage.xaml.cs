using System;
using System.IO;
using System.Text;
using Microsoft.Maui.Controls;
using Foundation;
using UIKit;
using System.Diagnostics;
using maui_app.Platforms.MacCatalyst.Controller;

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

           // コンボボックスの初期化
        private void InitializeComboBox()
        {
            try
            {
                // 動的にデータを追加
                var items = new List<string>
                {
                    "動的選択肢1",
                    "動的選択肢2",
                    "動的選択肢3"
                };

                ComboBoxPicker.ItemsSource = items;

                Console.WriteLine("コンボボックスを初期化しました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"コンボボックスの初期化中にエラーが発生しました: {ex.Message}");
            }
        }

        // コンボボックスの選択変更イベント
        private void OnComboBoxSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxPicker.SelectedIndex != -1)
                {
                    string selectedValue = ComboBoxPicker.Items[ComboBoxPicker.SelectedIndex];
                    Console.WriteLine($"選択された値: {selectedValue}");
                }
                else
                {
                    Console.WriteLine("選択肢が選択されていません。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"コンボボックスの選択処理中にエラーが発生しました: {ex.Message}");
            }
        }

        // ファイル選択ダイアログを表示
        private async void OnSelectFolderClicked(object sender, EventArgs e)
        {
            try
            {
                 if(DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    MacController macController = new MacController();
                    macController.ImportInputFile(SelectedFileContent);
                }
                else
                {
                    Debug.WriteLine("このデバイスは、対応していません。");
                }

                return;

                // CSVファイルを指定するためのUTI（Uniform Type Identifier）を設定
                // CSVファイルのみを選択できるようにする
                var allowedUTIs = new string[] { "public.comma-separated-values-text" };

                // UIDocumentPickerViewControllerを作成し、インポートモードを指定
                var documentPicker = new UIDocumentPickerViewController(allowedUTIs, UIDocumentPickerMode.Import);

                // ファイル選択後の処理を指定する。
                documentPicker.Delegate = new FilePickerDelegate(this);

                // ドキュメントピッカーの表示スタイルを全画面に設定
                documentPicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                // 現在表示されているUIViewControllerを取得
                var viewController = Platform.GetCurrentUIViewController();

                // UIViewControllerが取得できた場合、ドキュメントピッカーを表示
                if (viewController != null)
                {
                    await viewController.PresentViewControllerAsync(documentPicker, true);
                }
            }
            catch (Exception ex)
            {
                // エラーが発生した場合、エラーメッセージをコンソールに出力
                Console.WriteLine($"エラー: {ex.Message}");
            }
        }



        // ファイル選択ダイアログのデリゲートクラス
        private class FilePickerDelegate : UIDocumentPickerDelegate
        {
            private readonly MainPage mainPage; // MainPageインスタンスへの参照

            // コンストラクタでMainPageのインスタンスを受け取る
            public FilePickerDelegate(MainPage mainPage)
            {
                this.mainPage = mainPage;
            }

            // ユーザーがファイル選択をキャンセルしたときの処理
            public override void WasCancelled(UIDocumentPickerViewController controller)
            {
                // キャンセルが通知された際のログ出力
                Console.WriteLine("ファイル選択がキャンセルされました。");
            }

            // ユーザーがファイルを選択したときの処理
            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
            {
                // 選択されたファイルのURLリストが空でない場合の処理
                if (urls != null && urls.Length > 0)
                {
                    try
                    {
                        // 選択された最初のファイルのURLを取得
                        var fileUrl = urls[0];

                        // ファイルパスを取得
                        var filePath = fileUrl.Path ?? string.Empty;

                        // ファイルパスが空である場合のエラーハンドリング
                        if (filePath == string.Empty)
                        {
                            Console.WriteLine("ファイルパスが空です。");
                            return;
                        }

                        // ファイルの内容を読み込んでMainPageのプロパティに設定
                        mainPage.SelectedFileContent = File.ReadAllText(filePath);

                        // セキュリティスコープリソースの使用を終了
                        fileUrl.StopAccessingSecurityScopedResource();

                        // ファイル読み込み成功時のログ出力
                        Console.WriteLine("ファイルの内容を取得しました。");
                    }
                    catch (Exception ex)
                    {
                        // ファイル読み込み中に発生したエラーをキャッチしてログに出力
                        Console.WriteLine($"ファイルの読み込み中にエラーが発生しました: {ex.Message}");
                    }
                }
                else
                {
                    // ファイルが選択されなかった場合（キャンセルされた場合）の処理
                    Console.WriteLine("ファイル選択がキャンセルされました。");
                }
            }
        }

        // 選択されたCSVファイルをJSONに変換してダウンロード
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
    }
}
