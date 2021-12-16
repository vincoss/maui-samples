using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace ShellFlyoutPage_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            App.Current.UserAppTheme = OSAppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new MainPage() { Title = "ShellFlyoutPage_Samples" });
        }
    }
}
