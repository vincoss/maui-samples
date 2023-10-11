using Blazor_AppWithWebServer_EmbedIO.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Blazor_AppWithWebServer_EmbedIO
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
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IEmbedServer, EmbedIOServerService>();
            builder.Services.AddSingleton<IHttpRequestProvider, HttpRequestProvider>();

            return builder.Build();
        }
    }
}
