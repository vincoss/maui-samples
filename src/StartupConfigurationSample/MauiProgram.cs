using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Extensions.DependencyInjection;
using StartupConfigurationSample.Interfaces;
using StartupConfigurationSample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Essentials;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Console;
using System.Net.Http;
using System;

namespace StartupConfigurationSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
			var configuration = GetConfiguration(new string[0]);
			var builder = MauiApp.CreateBuilder();

			builder
				.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .Host.ConfigureServices((b, services) =>
                {
                    services.AddHttpClient(); // TODO: System.NullReferenceException: 'Object reference not set to an instance of an object.'
                    services.AddSingleton<IBootstrap, Bootstrap>();
                    services.AddSingleton<IDatabaseService, DatabaseService>();
                    services.AddSingleton<IHttpService, HttpService>();


                    ILoggerFactory loggerFactory = new LoggerFactory();

                    services.AddSingleton(loggerFactory);
                    services.AddLogging();

                    var httpClientHandler = new SocketsHttpHandler()
                    {
                        PooledConnectionLifetime = TimeSpan.FromMinutes(1)
                    };
                    services.AddSingleton(httpClientHandler);

                    services.AddTransient<HttpClient>((p) =>
                    {
                        var handler = p.GetService<SocketsHttpHandler>();
                        return new HttpClient(handler, false);
                    });

                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                });

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