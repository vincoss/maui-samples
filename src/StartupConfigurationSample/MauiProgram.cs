using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Logging;

using StartupConfigurationSample.Interfaces;
using StartupConfigurationSample.Services;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace StartupConfigurationSample
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

            var services = builder.Services;

            services.AddHttpClient();
            services.AddSingleton<IBootstrap, Bootstrap>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IHttpService, HttpService>();

            var loggerFactory = new LoggerFactory();
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            // Other sample
            //var httpClientHandler = new SocketsHttpHandler()
            //{
            //    PooledConnectionLifetime = TimeSpan.FromMinutes(1)
            //};
            //services.AddSingleton(httpClientHandler);

            //services.AddTransient<HttpClient>((p) =>
            //{
            //    var handler = p.GetService<SocketsHttpHandler>();
            //    return new HttpClient(handler, false);
            //});

            return builder.Build();
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(FileSystem.AppDataDirectory)
                        .AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" })
                        .AddJsonFile(new EmbeddedFileProvider(typeof(MauiProgram).Assembly), "appsettings.json", false, false)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);

            return builder.Build();
        }
    }
}