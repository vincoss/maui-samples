using Blazor_AppWithWebServer_EmbedIO.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Blazor_AppWithWebServer_EmbedIO.Controllers;


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
            builder.Services.AddSingleton<IHttpRequestProvider, HttpRequestProvider>();
            RegisterServerServices(builder.Services);

            return builder.Build();
        }

        private static void RegisterServerServices(IServiceCollection services)
        {
            services.AddSingleton<IPath, PathService>();
            services.AddSingleton<IEmbedServer, EmbedIOServerService>();

            var serverOptions = new ServerOptions();
            services.AddSingleton(serverOptions);

            services.AddTransient<TestController>();
        }
    }
}
