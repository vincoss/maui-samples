using Localization_Samples_Library;
using System.Globalization;

namespace Localization_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetupLocalization();

            MainPage = new MainPage();
        }

        private void SetupLocalization()
        {
            CultureApp.Cache.Add("App starting...");
            CultureApp.Cache.Add($"CurrentCulture: {CultureInfo.CurrentCulture}");
            CultureApp.Cache.Add($"CurrentUICulture: {CultureInfo.CurrentUICulture}");
            CultureApp.Cache.Add("Change App culture...");

            var ci = new CultureInfo("ru");
            AppResources.Culture = ci; // set the RESX for resource localization

            CultureInfo.CurrentCulture = ci;
            CultureInfo.CurrentUICulture = ci;
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            CultureApp.Cache.Add($"CurrentCulture: {CultureInfo.CurrentCulture}");
            CultureApp.Cache.Add($"CurrentUICulture: {CultureInfo.CurrentUICulture}");
        }
    }
}