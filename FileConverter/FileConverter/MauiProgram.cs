using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif

namespace FileConverter
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // アプリケーション起動時サイズ設定
#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        var nativeWindow = window.GetWindowHandle();
                        var windowId = Win32Interop.GetWindowIdFromWindow(nativeWindow);
                        var appWindow = AppWindow.GetFromWindowId(windowId);
                        appWindow.Resize(new Windows.Graphics.SizeInt32(800, 1000));
                    });
                });
            });
#endif

            return builder.Build();
        }
    }
}
