using Sqlite_Dapper_Samples.Interfaces;
using Sqlite_Dapper_Samples.Platforms;
using Sqlite_Dapper_Samples.Services;

//#if ANDROID
//    using Sqlite_Dapper_Samples.Platforms.Android;
//#endif
//#if WINDOWS
//    using Sqlite_Dapper_Samples.Platforms.Windows;
//#endif
//#if IOS
//    using Sqlite_Dapper_Samples.Platforms.iOS;
//#endif
//#if MACCATALYST
//   using Sqlite_Dapper_Samples.Platforms.MacCatalyst;
//#endif

namespace Sqlite_Dapper_Samples
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
            services.AddSingleton<IItemService, ItemService>();
            services.AddTransient<ProductListViewModel>();

            return builder.Build();
        }
    }
}