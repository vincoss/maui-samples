using Microsoft.Extensions.DependencyInjection;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private Uri? _pendingDeepLink;

        protected override void OnStart()
        {
            base.OnStart();

            if (_pendingDeepLink != null)
            {
                var uri = _pendingDeepLink;
                _pendingDeepLink = null;
                HandleDeepLink(uri);
            }
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            if (Shell.Current == null)
            {
                _pendingDeepLink = uri;
                return;
            }

            HandleDeepLink(uri);
        }

        private async void HandleDeepLink(Uri uri)
        {
            CacheTemp.Url = uri?.ToString();

            //// Example URI: mauiapp1://products/42
            //if (uri.Scheme.Equals("mauiapp1", StringComparison.OrdinalIgnoreCase) &&
            //    uri.Host.Equals("products", StringComparison.OrdinalIgnoreCase))
            //{
            //    var productId = uri.AbsolutePath.Trim('/');
            //    await Shell.Current.GoToAsync($"//product-details?id={productId}");
            //}
        }
    }

    public static class  CacheTemp
    {
        public static string Url { get; set; }
    }

    /*
        Windows (PowerShell): Start-Process "mauiapp1://products/42"
        Android (ADB): adb shell am start -W -a android.intent.action.VIEW -d "mauiapp1://products/42"
        iOS Simulator: xcrun simctl openurl booted "mauiapp1://products/42"
     */
}