using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Sqlite_EF_Samples.Platforms.Android;
using Sqlite_EF_Samples_Library.Interfaces;
using Sqlite_EF_Samples_Library.Services;

namespace Sqlite_EF_Samples
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
                .Host.ConfigureServices((b, services) =>
                {
                    services.AddSingleton<IPath, DbPath>();
                    services.AddSingleton<IBootstrap, Bootstrap>();
                    services.AddSingleton<IDataMigrations, SqliteDataMigrations>();
                    services.AddSingleton<IDatabaseService, DatabaseService>();
                });

            return builder.Build();
        }
    }
}