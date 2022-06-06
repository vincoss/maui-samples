using Sqlite_EF_Samples.Platforms;
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            var services = builder.Services;

            services.AddSingleton<IPath, DbPath>();
            services.AddSingleton<IBootstrap, Bootstrap>();
            services.AddSingleton<IDataMigrations, SqliteDataMigrations>();
            services.AddSingleton<IDatabaseService, DatabaseService>();

            return builder.Build();
        }
    }
}