using System.Diagnostics;

#if WINDOWS
using Windows.Storage;
using Windows.Storage.Pickers;
#endif


namespace FileConverter
{
    public class UserInterface
    {
        #region フィールド変数

        /// <summary>選択ファイル名</summary>
        private string _fileFullPath = string.Empty;

        /// <summary>入力ファイル形式</summary>
        private string _inputFormat = string.Empty;

        /// <summary>出力ファイル形式</summary>
        private string _outputMode = string.Empty;

        /// <summary>拡張子チェック用配列</summary>
        private string[] _extensions = ["CSV", "JSON", "SQL"];

        #endregion

        #region プロパティ

        /// <summary>選択ファイル名</summary>
        public string FileFullPath
        {
            get
            {
                return _fileFullPath;
            }
        }

        /// <summary>入力ファイル形式</summary>
        public string InputFormat
        {
            get
            {
                return _inputFormat;
            }
        }

        /// <summary>出力ファイル形式</summary>
        public string OutputFormat
        {
            get
            {
                return _outputMode;
            }
            set
            {
                _outputMode = value;
            }
        }

        #endregion

        /// <summary>
        /// ファイル選択
        /// </summary>
        public async Task<int> SelectFile()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    // ファイル拡張の取得
                    _inputFormat = Path.GetExtension(result.FullPath)[1..].ToUpper();

                    if (_extensions.Contains(_inputFormat))
                    {
                        // パスを設定
                        _fileFullPath = result.FullPath;

                        // 正常
                        return 1;
                    }
                    else
                    {
                        // 異常
                        return 2;
                    }
                }

                // 未選択
                return 0;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 出力ファイル形式選択
        /// </summary>
        /// <returns></returns>
        public void SelectOutputFormat(string outputFormat)
        {
            _outputMode = outputFormat;
        }

        /// <summary>
        /// ファイル保存
        /// </summary>
        /// <returns></returns>
        public static async Task SaveFileAsync(string outputFormat, string data)
        {
            string filePath = string.Empty;

            try
            {
#if WINDOWS
                var savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("テキストファイル", new List<string>() { $".{outputFormat.ToLower()}" });

                var window = ((Microsoft.UI.Xaml.Window?)Application.Current?.Windows[0].Handler.PlatformView);
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    filePath = file.Path;
                }
#endif
                // 保存先のファイルに書き込み
                using var writer = new StreamWriter(filePath, false);
                writer.Write(data);
                writer.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
