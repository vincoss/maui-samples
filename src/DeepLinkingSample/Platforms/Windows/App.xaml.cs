using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MauiApp1.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var mainInstance = AppInstance.FindOrRegisterForKey("main");
            var activatedArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

            if (!mainInstance.IsCurrent)
            {
                await mainInstance.RedirectActivationToAsync(activatedArgs);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            mainInstance.Activated += OnAppInstanceActivated;

            base.OnLaunched(args);

            HandleActivationArgs(activatedArgs);
        }

        private void OnAppInstanceActivated(object? sender, AppActivationArguments args)
        {
            Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(() =>
            {
                HandleActivationArgs(args);
                BringWindowToFront();
            });
        }

        private static void HandleActivationArgs(AppActivationArguments args)
        {
            if (args.Kind == ExtendedActivationKind.Protocol &&
                args.Data is IProtocolActivatedEventArgs protocolArgs &&
                protocolArgs.Uri != null)
            {
                Microsoft.Maui.Controls.Application.Current?.SendOnAppLinkRequestReceived(protocolArgs.Uri);
            }
        }

        private static void BringWindowToFront()
        {
            var window = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault()?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            window?.Activate();
        }
    }

}
