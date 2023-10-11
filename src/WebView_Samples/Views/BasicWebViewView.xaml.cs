namespace MauiApp1;

public partial class BasicWebViewView : ContentPage
{
	public BasicWebViewView()
	{
		InitializeComponent();
	}

    private void webviewNavigating(object sender, WebNavigatingEventArgs e)
    {
        labelLoading.IsVisible = true;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        webView.Reload();
    }

    private void webView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        labelLoading.IsVisible = false;
    }
}