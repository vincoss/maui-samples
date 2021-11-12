using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;


namespace Sqlite_Dapper_Samples.Platforms.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}