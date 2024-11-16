using System;
using System.Runtime.InteropServices;

namespace Interop
{
    /// <summary>
    /// Rustの関数を呼び出すためのクラス
    /// </summary>
    public static class RustFunctions
    {
        // CSV → JSON 変換関数を呼び出す
        public static IntPtr ConvertCsvToJson(string csvData)
        {
            IntPtr functionPtr = RustLibraryLoader.GetFunctionPointer("csv_to_json");
            var csvToJsonFunc = Marshal.GetDelegateForFunctionPointer<ConvertCsvToJsonDelegate>(functionPtr);

            return csvToJsonFunc(csvData);
        }

        // JSON → CSV 変換関数を呼び出す
        public static IntPtr ConvertJsonToCsv(string jsonData)
        {
            IntPtr functionPtr = RustLibraryLoader.GetFunctionPointer("json_to_csv");
            var jsonToCsvFunc = Marshal.GetDelegateForFunctionPointer<ConvertJsonToCsvDelegate>(functionPtr);

            return jsonToCsvFunc(jsonData);
        }

        // CSV → SQLクエリ変換関数を呼び出す
        public static IntPtr ConvertCsvToQuery(string csvData, string tableName)
        {
            IntPtr functionPtr = RustLibraryLoader.GetFunctionPointer("csv_to_query");
            var csvToQueryFunc = Marshal.GetDelegateForFunctionPointer<ConvertCsvToQueryDelegate>(functionPtr);

            return csvToQueryFunc(csvData, tableName);
        }

        // SQLクエリ → CSV 変換関数を呼び出す（追加したメソッド）
        public static IntPtr ConvertQueryToCsv(string queryData)
        {
            IntPtr functionPtr = RustLibraryLoader.GetFunctionPointer("query_to_csv");
            var queryToCsvFunc = Marshal.GetDelegateForFunctionPointer<ConvertQueryToCsvDelegate>(functionPtr);

            return queryToCsvFunc(queryData); // SQLクエリデータを変換してポインタを返す
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
        private delegate IntPtr ConvertJsonToCsvDelegate(string jsonData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertCsvToQueryDelegate(string csvData, string tableName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ConvertQueryToCsvDelegate(string queryData); 
    }
}