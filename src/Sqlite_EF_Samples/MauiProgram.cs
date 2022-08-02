using Microsoft.Data.Sqlite;
using Sqlite_EF_Samples.Platforms;
using Sqlite_EF_Samples_Library.Interfaces;
using Sqlite_EF_Samples_Library.Services;
using System.Data.Common;

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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            var services = builder.Services;

            services.AddSingleton<IPath, DbPath>();
            services.AddSingleton<IBootstrap, Bootstrap>();
            services.AddSingleton<IDataMigrations, SqliteDataMigrations>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton((services) =>
            {
                var service = services.GetService<IDatabaseService>();
                var connection = new SqliteConnection(service.ConnectionString);
                connection.Open();
                return (DbConnection)connection;
            });

            return builder.Build();
        }
    }
}