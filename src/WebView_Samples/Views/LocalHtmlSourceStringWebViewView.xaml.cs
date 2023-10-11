namespace MauiApp1;

public partial class LocalHtmlSourceStringWebViewView : ContentPage
{
	public LocalHtmlSourceStringWebViewView()
	{
		InitializeComponent();

        webViewCode.Source = new HtmlWebViewSource
        {
            Html = @"<HTML><BODY><H1>.NET MAUI C#</H1><P>Welcome to WebView.</P></BODY></HTML>"
        };
    }
}