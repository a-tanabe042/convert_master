using Conversions;
using System.Diagnostics;

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
                if (_userInterface.FileFullPath != string.Empty && _userInterface.OutputFormat != string.Empty)
                {
                    // ファイル内容取得
                    string content = File.ReadAllText(_userInterface.FileFullPath);

                    // 変換文字列取得
                    string data = GetConvertMode(_userInterface.InputFormat, _userInterface.OutputFormat, content);

                    // 変換データ出力
                    await UserInterface.SaveFileAsync(_userInterface.OutputFormat, data);

                    await DisplayAlert("【通知】", "変換が完了しました。", "OK");
                }
                else
                {
                    await DisplayAlert("【通知】", "ファイルが選択されていません。", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("【例外通知】", ex.ToString(), "閉じる");
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
                return JsonToQueryConverter.Convert(data);
            }
            else if (inputFormat == SQL && outputFormat == CSV)
            {
                return QueryToCsvConverter.Convert(data);
            }
            else if (inputFormat == SQL && outputFormat == JSON)
            {
                return QueryToJsonConverter.Convert(data);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ラジオボタン(CSV)選択時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedCsv(object sender, CheckedChangedEventArgs e)
        {
            // エラーメッセージを初期化
            ErrorLabel01.Text = string.Empty;

            // 入力形式がCSVの場合
            if (_userInterface.InputFormat == CSV)
            {
                // エラーメッセージを画面に表示する。
                ErrorLabel01.Text = "JSONまたは、SQLを選択してください。";
                return;
            }

            _userInterface.SelectOutputFormat(CSV);
        }

        /// <summary>
        /// ラジオボタン(JSON)選択時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedJson(object sender, CheckedChangedEventArgs e)
        {
            // エラーメッセージを初期化
            ErrorLabel01.Text = string.Empty;

            // 入力形式がJSONの場合
            if (_userInterface.InputFormat == JSON)
            {
                // エラーメッセージを画面に表示する。
                ErrorLabel01.Text = "CSVまたは、SQLを選択してください。";
                return;
            }

            _userInterface.SelectOutputFormat(JSON);
        }

        /// <summary>
        /// ラジオボタン(QUERY)選択時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedSql(object sender, CheckedChangedEventArgs e)
        {
            // エラーメッセージを初期化
            ErrorLabel01.Text = string.Empty;

            // 入力形式がSQLの場合
            if (_userInterface.InputFormat == SQL)
            {
                // エラーメッセージを画面に表示する。
                ErrorLabel01.Text = "CSVまたは、JSONを選択してください。";
                return;
            }

            _userInterface.SelectOutputFormat(SQL);
        }
    }
}
