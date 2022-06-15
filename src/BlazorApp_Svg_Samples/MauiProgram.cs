using BlazorApp_Svg_Samples.Data;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace BlazorApp_Svg_Samples
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
                    fonts.AddFont("MaterialIconsSharp-Regular.otf", "MaterialDesign");
                    fonts.AddFont("Font-Awesome-5-Free-Solid-900.otf", "FontAwesome");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}