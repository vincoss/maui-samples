using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Default_MauiApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}