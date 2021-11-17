using Geolocation_Samples.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace Geolocation_Samples
{
    public partial class App : Application
    {
        public static readonly GeolocationService GeoService = new GeolocationService();
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new MainPage())) { Title = "Geolocation_Samples" };
        }

        protected override void OnStart()
        {
            GeoService.Run();
        }
    }
}
