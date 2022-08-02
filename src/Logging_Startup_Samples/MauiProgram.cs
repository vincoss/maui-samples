using Logging_Startup_Samples.Interface;
using Logging_Startup_Samples.Services;

namespace Logging_Startup_Samples
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

            builder.Services.AddTransient<IPath, PathService>();
            builder.Services.AddTransient<Startup>();

            var app = builder.Build();
            App.ServiceProvider = app.Services;

            return app;
        }
    }
}