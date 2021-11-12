using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;


namespace Localization_Samples.Platforms.MacCatalyst
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}