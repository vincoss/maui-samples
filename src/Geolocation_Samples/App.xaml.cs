using Geolocation_Samples.Services;

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