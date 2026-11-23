using Blazor_AppWithWebServer_EmbedIO.Services;

namespace Blazor_AppWithWebServer_EmbedIO.Pages;

public partial class WebViewPage : ContentPage
{
	public WebViewPage()
	{
		InitializeComponent();

        var model = App.ServiceProvider.GetService<WebViewPageViewModel>();
        BindingContext = model;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await this.Navigation.PopAsync();
    }

    public class WebViewPageViewModel
    {
        private readonly IEmbedServer _embedServer;


        public WebViewPageViewModel(IEmbedServer embedServer)
        {
            _embedServer = embedServer ?? throw new ArgumentNullException(nameof(embedServer));
        }

        public string SourceUrl
        {
            get
            {
                var url = $"{_embedServer.GetBaseUrl()}/index.html";
                return url;
            }
        }
    }
}