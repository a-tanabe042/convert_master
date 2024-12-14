using Conversions;
using System.Diagnostics;

namespace FileConverter
{
    public partial class MainPage : ContentPage
    {
        const string CSV = "CSV";
        const string JSON = "JSON";
        const string SQL = "SQL";

        /// <summary>変換モード</summary>
        private enum ConvertMode
        {
            CsvToJSON,
            CsvToSQL,
            JSONToCsv,
            JSONToSQL,
            SQLToCsv,
            SQLToJSON,
            NONE
        }

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

            // Pickerの初期表示設定
            OutputFormatPicker.SelectedIndex = 0;
        }

        /// <summary>
        /// ファイル選択ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSelectFileClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("ファイル選択ボタンが押下されました。");

            await _userInterface.SelectFile();

            // ラベルに選択したファイル名を表示
            FilePathLabel.Text = _userInterface.FileFullPath;
        }

        /// <summary>
        /// 変換ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnConvertButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("変換ボタンが押下されました。");

            // ファイル内容取得
            string content = File.ReadAllText(_userInterface.FileFullPath);

            // 変換文字列取得
            string data = GetConvertMode(_userInterface.InputFormat, _userInterface.OutputFormat, content);

            // 変換データ出力
            await UserInterface.SaveFileAsync(_userInterface.OutputFormat, data);
        }

        /// <summary>
        /// 出力ファイル形式変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("出力ファイル形式が変更されました。");

            _userInterface.SelectOutputFormat((Picker)sender);
        }

        /// <summary>
        /// 変換文字列取得
        /// </summary>
        /// <returns>変換文字列</returns>
        private string GetConvertMode(string inputFormat, string outputFormat, string data)
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
    }
}
