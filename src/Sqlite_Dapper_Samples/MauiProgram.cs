using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqlite_Dapper_Samples.Interfaces;
using Sqlite_Dapper_Samples.Services;
using Sqlite_Dapper_Samples;


namespace Sqlite_Dapper_Samples
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                //.UseMauiServiceProviderFactory(true) // TODO

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .Host.ConfigureServices((d, services) =>
                {
                    services.AddSingleton<IPath, DbPath>();
                    services.AddSingleton<IBootstrap, Bootstrap>();
                    services.AddSingleton<IDataMigrations, SqliteDataMigrations>();
                }); ;

            return builder.Build();
        }
    }
}