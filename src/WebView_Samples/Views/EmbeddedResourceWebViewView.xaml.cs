using System.Reflection;

namespace MauiApp1;

public partial class EmbeddedResourceWebViewView : ContentPage
{
	public EmbeddedResourceWebViewView()
	{
		InitializeComponent();
        HtmlSource();
    }

    private void HtmlSource()
    {
        var htmlSource = new HtmlWebViewSource();
        var path = "WebView_Samples.Embedded.page001.html";
        htmlSource.Html = SampleExtensions.GetValueEmbeddedResource(typeof(EmbeddedResourceWebViewView).Assembly, path);
        webView.Source = htmlSource;
    }

    public static class SampleExtensions
    {
        public static string GetValueEmbeddedResource(Assembly assembly, string path)
        {
            using (var stream = assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}