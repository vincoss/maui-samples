using Blazor_AppWithWebServer_EmbedIO.Pages;

namespace Blazor_AppWithWebServer_EmbedIO
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void BlazorButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BlazorViewPage());
        }

        private async void WebViewButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WebViewPage());
        }
    }
}
