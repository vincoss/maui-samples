using Entry_Samples.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace Entry_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new MainPage())) { Title = "Geolocation_Samples" };
        }

    }
}
