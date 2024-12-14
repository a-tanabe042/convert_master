using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;

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
        }

        #endregion

        /// <summary>
        /// ファイル選択
        /// </summary>
        public async Task SelectFile()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    _fileFullPath = result.FullPath;

                    // ファイル拡張の取得
                    _inputFormat = Path.GetExtension(result.FullPath)[1..].ToUpper();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
            }
        }

        /// <summary>
        /// 出力ファイル形式選択
        /// </summary>
        /// <returns></returns>
        public bool SelectOutputFormat(Picker picker)
        {
            var selectedFormat = (string)picker.SelectedItem;
            Debug.WriteLine($"出力ファイル形式が変更されました。: {selectedFormat}");

            _outputMode = selectedFormat;

            return true;
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
