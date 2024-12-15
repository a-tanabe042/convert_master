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

                    // Pickerの初期化
                    InitialPicker(_userInterface.InputFormat);

                    // Pickerの初期選択値を出力ファイル形式に設定
                    _userInterface.OutputFormat = (string)OutputFormatPicker.SelectedItem;
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
                await DisplayAlert("【例外通知】", ex.Message, "閉じる");
            }
        }

        /// <summary>
        /// 出力ファイル形式変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _userInterface.SelectOutputFormat((Picker)sender);
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

        /// <summary>
        /// Pickerの初期化
        /// </summary>
        /// <param name="inputFormat">入力ファイル形式</param>
        private void InitialPicker(string inputFormat)
        {
            OutputFormatPicker.Items.Clear();

            if (inputFormat == CSV)
            {
                OutputFormatPicker.Items.Add("JSON");
                OutputFormatPicker.Items.Add("SQL");
            }
            else if (inputFormat == JSON)
            {
                OutputFormatPicker.Items.Add("CSV");
                OutputFormatPicker.Items.Add("SQL");
            }
            else
            {
                OutputFormatPicker.Items.Add("CSV");
                OutputFormatPicker.Items.Add("JSON");
            }

            // 初期選択
            OutputFormatPicker.SelectedIndex = 0;
        }
    }
}
