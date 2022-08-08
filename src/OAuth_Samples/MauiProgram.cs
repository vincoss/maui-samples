using OAuth_Samples.Pages;
using OAuth_Samples.Security;
using OAuth_Samples.ViewModels;

namespace OAuth_Samples
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

            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddSingleton(new AuthClient(new()
            {
                Domain = "devidentity.plexer.com.au",//"localhost:5010",
                ClientId = "plexer-api",
                Scope = "plexerapi openid",
#if WINDOWS
			RedirectUri = "http://localhost/callback"
#else
                RedirectUri = "oauthsamples://callback",
#endif
            }));

            var app = builder.Build();
            App.ServiceProvider = app.Services;

            return app;
        }
    }
}