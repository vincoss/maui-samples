using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Validation_Samples.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace Validation_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new EditSamplesView())) { Title = "MyApp" };
        }
    }
}
