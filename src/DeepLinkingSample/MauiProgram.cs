using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Inside CreateMauiApp():
            builder.ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android => android
                    .OnCreate((activity, bundle) => HandleAndroidIntent(activity.Intent))
                    .OnNewIntent((activity, intent) => HandleAndroidIntent(intent)));

                static void HandleAndroidIntent(Android.Content.Intent? intent)
                {
                    if (intent?.Action == Android.Content.Intent.ActionView && intent.DataString is string uriString)
                    {
                        if (Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
                            Microsoft.Maui.Controls.Application.Current?.SendOnAppLinkRequestReceived(uri);
                    }
                }
#elif IOS || MACCATALYST
    events.AddiOS(ios => ios
        .OpenUrl((app, url, options) =>
        {
            if (Uri.TryCreate(url.AbsoluteString, UriKind.Absolute, out var uri))
            {
                Microsoft.Maui.Controls.Application.Current?.SendOnAppLinkRequestReceived(uri);
                return true;
            }
            return false;
        }));
#endif
            });



            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
