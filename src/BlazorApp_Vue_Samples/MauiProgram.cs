using BlazorApp_Vue_Samples.Data;
using BlazorApp_Vue_Samples.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace BlazorApp_Vue_Samples
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
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddTransient<JsConsole>();

            return builder.Build();
        }
    }
}