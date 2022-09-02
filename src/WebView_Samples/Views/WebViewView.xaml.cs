namespace MauiApp1;

public partial class WebViewView : ContentPage
{
	public WebViewView(string url)
	{
		InitializeComponent();

		webView.Source = url;
	}
}