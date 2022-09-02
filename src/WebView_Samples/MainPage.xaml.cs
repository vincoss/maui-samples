using MauiApp1;

namespace WebView_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var url = entryUrl.Text;

            if(string.IsNullOrWhiteSpace(url))
            {
                this.DisplayAlert("Alert", "Enter a valid URL.", "OK");
                return;
            }
            this.Navigation.PushAsync(new WebViewView(url));
        }
    }
}