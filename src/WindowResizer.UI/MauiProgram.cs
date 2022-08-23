using Microsoft.AspNetCore.Components.WebView.Maui;
using WindowResizer.UI.Data;
using WindowResizer.UI.Services;

namespace WindowResizer.UI
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
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials
                        .AddAppAction("Settings", "Open settings", icon: "appicon")
                        .OnAppAction(App.HandleAppActions);
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

#if WINDOWS
            builder.Services.AddSingleton<ITrayContext, Platforms.Windows.TrayContext>();
            builder.Services.AddSingleton<INotificationService, Platforms.Windows.NotificationService>();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}
