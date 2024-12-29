using Conversions;
using System.Diagnostics;
using Foundation;
using System.Text;
using UIKit;
using Microsoft.Maui.Storage;
using System.IO;

namespace FileConverter
{
    public partial class MainPage : ContentPage
    {
        const string CSV = "CSV";
        const string JSON = "JSON";
        const string SQL = "SQL";

        /// <summary>UserInterfaceクラス</summary>
        private readonly UserInterface _userInterface;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // UserInterfaceクラスの初期化
            _userInterface = new();
        }

        /// <summary>
        /// ファイル選択ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSelectFileClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("ファイル選択ボタンが押下されました。");

            try
            {
                int result = await _userInterface.SelectFile();

                // ファイルを選択した場合
                if (result == 1)
                {
                    // ラベルに選択したファイル名を表示
                    FilePathLabel.Text = _userInterface.FileFullPath;

                    // ラジオボタンをすべて活性状態にする
                    CsvRadioBtn.IsEnabled = true;
                    JsonRadioBtn.IsEnabled = true;
                    SqlRadioBtn.IsEnabled = true;
                }
                else if (result == 2)
                {
                    await DisplayAlert("【通知】", "CSV, JSON, SQL形式のファイルを選択してください。", "OK");
                }
                else
                {
                    // 何もしない。
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("【例外通知】", ex.Message, "閉じる");
            }
        }

        /// <summary>
        /// 変換ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnConvertButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("変換ボタンが押下されました。");

            try
            {
                if (!string.IsNullOrEmpty(_userInterface.FileFullPath) && !string.IsNullOrEmpty(_userInterface.OutputFormat))
                {
                    // ファイル内容を取得
                    string content = File.ReadAllText(_userInterface.FileFullPath);

                    // 変換文字列を取得
                    string data = GetConvertMode(_userInterface.InputFormat, _userInterface.OutputFormat, content);

                    // JSONデータを一時ファイルに書き込み
                    string tempFileName = $"output_{Guid.NewGuid()}.{_userInterface.OutputFormat.ToLower()}";
                    string tempFilePath = Path.Combine(FileSystem.CacheDirectory, tempFileName);
                    byte[] fileBytes = Encoding.UTF8.GetBytes(data);
                    await File.WriteAllBytesAsync(tempFilePath, fileBytes);

                    // UIDocumentPickerを使用してエクスポート
                    var fileUrl = new NSUrl(tempFilePath, false);
                    var documentPicker = new UIDocumentPickerViewController(new[] { fileUrl }, UIDocumentPickerMode.ExportToService)
                    {
                        Delegate = new DocumentPickerDelegate(),
                        ModalPresentationStyle = UIModalPresentationStyle.FullScreen
                    };

                    var viewController = Platform.GetCurrentUIViewController();
                    if (viewController != null)
                    {
                        await viewController.PresentViewControllerAsync(documentPicker, true);
                    }

                    ConvertButton.Text = "変換が完了しました！ ファイルを保存しました。";
                }
                else
                {
                    await DisplayAlert("【通知】", "ファイルが選択されていません。", "OK");
                }
            }
            catch (Exception ex)
            {
                ConvertButton.Text = $"エラー: {ex.Message}";
            }
        }
        /// <summary>
        /// 変換文字列取得
        /// </summary>
        /// <returns>変換文字列</returns>
        private static string GetConvertMode(string inputFormat, string outputFormat, string data)
        {
            if (inputFormat == CSV && outputFormat == JSON)
            {
                return CsvToJsonConverter.Convert(data);
            }
            else if (inputFormat == CSV && outputFormat == SQL)
            {
                return CsvToQueryConverter.Convert(data, "SampleTbl");
            }
            else if (inputFormat == JSON && outputFormat == CSV)
            {
                return JsonToCsvConverter.Convert(data);
            }
            else if (inputFormat == JSON && outputFormat == SQL)
            {
                // Rustの処理で、例外発生？
                return JsonToQueryConverter.Convert(data);
            }
            else if (inputFormat == SQL && outputFormat == CSV)
            {
                // Rustの処理で、例外発生？
                return QueryToCsvConverter.Convert(data);
            }
            else if (inputFormat == SQL && outputFormat == JSON)
            {
                // Rustの処理で、例外発生？
                return QueryToJsonConverter.Convert(data);
            }
            else
            {
                return "";
            }
        }

        private void CheckedCsv(object sender, CheckedChangedEventArgs e)
        {
            Debug.WriteLine($"CheckedCsv called - IsChecked: {e.Value}");

            // 他のRadioButtonのIsCheckedをfalseにする
            JsonRadioBtn.IsChecked = false;
            SqlRadioBtn.IsChecked = false;

            if (e.Value) // CSVが選択された場合
            {
                ErrorLabel01.Text = string.Empty;

                // 入力形式がCSVの場合、エラーメッセージを表示
                if (_userInterface.InputFormat == CSV)
                {
                    ErrorLabel01.Text = "JSONまたは、SQLを選択してください。";
                }
                else
                {
                    _userInterface.SelectOutputFormat(CSV);
                }
            }
        }

        private void CheckedJson(object sender, CheckedChangedEventArgs e)
        {
            Debug.WriteLine($"CheckedJson called - IsChecked: {e.Value}");

            // 他のRadioButtonのIsCheckedをfalseにする
            CsvRadioBtn.IsChecked = false;
            SqlRadioBtn.IsChecked = false;

            if (e.Value) // JSONが選択された場合
            {
                ErrorLabel01.Text = string.Empty;

                // 入力形式がJSONの場合、エラーメッセージを表示
                if (_userInterface.InputFormat == JSON)
                {
                    ErrorLabel01.Text = "CSVまたは、SQLを選択してください。";
                }
                else
                {
                    _userInterface.SelectOutputFormat(JSON);
                }
            }
        }

        private void CheckedSql(object sender, CheckedChangedEventArgs e)
        {
            Debug.WriteLine($"CheckedSql called - IsChecked: {e.Value}");

            // 他のRadioButtonのIsCheckedをfalseにする
            CsvRadioBtn.IsChecked = false;
            JsonRadioBtn.IsChecked = false;

            if (e.Value) // SQLが選択された場合
            {
                ErrorLabel01.Text = string.Empty;

                // 入力形式がSQLの場合、エラーメッセージを表示
                if (_userInterface.InputFormat == SQL)
                {
                    ErrorLabel01.Text = "CSVまたは、JSONを選択してください。";
                }
                else
                {
                    _userInterface.SelectOutputFormat(SQL);
                }
            }
        }
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
}
