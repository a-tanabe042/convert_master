using System;
using System.Runtime.InteropServices;
using System.IO;

namespace Interop
{
    // <summary>
    // Rustのライブラリをロードするためのクラス
    // </summary>
    // <remarks>
    // Rustのライブラリをロードし、エクスポートされた関数のポインタを取得する
    // ライブラリ名はプラットフォームに応じて変更される
    // windows: librust_app.dll
    // linux: librust_app.so
    // mac: librust_app.dylib
    // </remarks>
    public static class RustLibraryLoader
    {
        private static IntPtr _libraryHandle;

        // プラットフォームに応じてライブラリを選択
        private static string GetLibraryName()
        {
            // return "librust_app.dll";
            // return "librust_app.so";
            return "librust_app.dylib";
        }

        // ライブラリのロード
        public static void LoadLibrary()
        {
            // ライブラリがロードされていない場合のみロード
            if (_libraryHandle == IntPtr.Zero)
            {
                // ライブラリ名を取得
                string libraryName = GetLibraryName();

                string libraryPath = Path.Combine("Contents/Resources/dll", libraryName);

                // ライブラリが存在しない場合はエラー
                if (!File.Exists(libraryPath))
                {
                    throw new DllNotFoundException($"ライブラリが見つかりません: {libraryPath}");
                }

                // ライブラリをロード
                _libraryHandle = NativeLibrary.Load(libraryPath);
            }
        }

        // エクスポートされた関数ポインタを取得する
        public static IntPtr GetFunctionPointer(string functionName)
        {
            LoadLibrary();
            return NativeLibrary.GetExport(_libraryHandle, functionName);
        }
    }
}