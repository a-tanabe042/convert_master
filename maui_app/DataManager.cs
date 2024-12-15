using System.Runtime.InteropServices;

namespace FileConverter
{
    public static class DataManager
    {
        private static IntPtr _libraryHandle;

        // プラットフォームに応じてライブラリを選択
        private static string GetLibraryName()
        {
#if WINDOWS
            return "librust_app.dll"; // Windows用のライブラリ
#elif LINUX
            return "librust_app.so"; // Linux用のライブラリ
#elif MACCATALYST
            return "librust_app.dylib"; // Mac Catalyst用のライブラリ
#else
            return "librust_app.dll"; // Windows用のライブラリ
#endif
        }

          // ライブラリのフルパスを取得
 private static string GetLibraryPath()
{
    // Mac Catalyst 用固定パス
    return Path.Combine(AppContext.BaseDirectory, "Contents", "Resources", "assets", "dll", GetLibraryName());
}
        // ライブラリのロード
         public static void LoadLibrary()
        {
            if (_libraryHandle == IntPtr.Zero)
            {
                string libraryPath = GetLibraryPath();

                // ライブラリが存在しない場合
                if (!File.Exists(libraryPath))
                {
                    Console.WriteLine($"ライブラリが見つかりません: {libraryPath}");
                    throw new DllNotFoundException($"ライブラリが見つかりません: {libraryPath}");
                }

                // ライブラリをロード
                try
                {
                    _libraryHandle = NativeLibrary.Load(libraryPath);
                    Console.WriteLine($"ライブラリが正常にロードされました: {libraryPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ライブラリのロード中にエラーが発生しました: {ex.Message}");
                    throw;
                }
            }
        }

        // エクスポートされた関数ポインタを取得する
        public static IntPtr GetFunctionPointer(string functionName)
        {
            LoadLibrary();
            return NativeLibrary.GetExport(_libraryHandle, functionName);
        }

        // CSV → JSON 変換関数を呼び出す
        public static IntPtr ConvertCsvToJson(string csvData)
        {
            IntPtr functionPtr = GetFunctionPointer("csv_to_json");
            var csvToJsonFunc = Marshal.GetDelegateForFunctionPointer<ConvertCsvToJsonDelegate>(functionPtr);

            return csvToJsonFunc(csvData);
        }

        // CSV → SQLクエリ変換関数を呼び出す
        public static IntPtr ConvertCsvToQuery(string csvData, string tableName)
        {
            IntPtr functionPtr = GetFunctionPointer("csv_to_query");
            var csvToQueryFunc = Marshal.GetDelegateForFunctionPointer<ConvertCsvToQueryDelegate>(functionPtr);

            return csvToQueryFunc(csvData, tableName);
        }

        // JSON → CSV 変換関数を呼び出す
        public static IntPtr ConvertJsonToCsv(string jsonData)
        {
            IntPtr functionPtr = GetFunctionPointer("json_to_csv");
            var jsonToCsvFunc = Marshal.GetDelegateForFunctionPointer<ConvertJsonToCsvDelegate>(functionPtr);

            return jsonToCsvFunc(jsonData);
        }

        // JSON → SQLクエリ変換関数を呼び出す
        public static IntPtr ConvertJsonToQuery(string jsonData)
        {
            IntPtr functionPtr = GetFunctionPointer("json_to_query");
            var jsonToQueryFunc = Marshal.GetDelegateForFunctionPointer<ConvertJsonToQueryDelegate>(functionPtr);

            return jsonToQueryFunc(jsonData);
        }

        // SQLクエリ → CSV 変換関数を呼び出す
        public static IntPtr ConvertQueryToCsv(string queryData)
        {
            IntPtr functionPtr = GetFunctionPointer("query_to_csv");
            var queryToCsvFunc = Marshal.GetDelegateForFunctionPointer<ConvertQueryToCsvDelegate>(functionPtr);

            return queryToCsvFunc(queryData);
        }

        // SQLクエリ → JSON 変換関数を呼び出す
        public static IntPtr ConvertQueryToJson(string queryData)
        {
            IntPtr functionPtr = GetFunctionPointer("query_to_json");
            var queryToJsonFunc = Marshal.GetDelegateForFunctionPointer<ConvertQueryToJsonDelegate>(functionPtr);

            return queryToJsonFunc(queryData);
        }

        // Rust側で確保したメモリを解放する
        public static void FreeRustString(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero) return;

            IntPtr functionPtr = GetFunctionPointer("free_rust_string");
            var freeStringFunc = Marshal.GetDelegateForFunctionPointer<FreeRustStringDelegate>(functionPtr);

            freeStringFunc(pointer);
        }

        // ポインタを文字列に変換する
        public static string ConvertPointerToString(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            return Marshal.PtrToStringAnsi(pointer);
        }

        // デリゲートの定義
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertCsvToJsonDelegate(string csvData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertCsvToQueryDelegate(string csvData, string tableName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertJsonToCsvDelegate(string jsonData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertJsonToQueryDelegate(string jsonData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertQueryToCsvDelegate(string queryData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertQueryToJsonDelegate(string queryData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreeRustStringDelegate(IntPtr pointer);
    }
}
