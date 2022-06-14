using BlazorApp_MixHtmlAndXamlPages_Samples.Pages;


namespace BlazorApp_MixHtmlAndXamlPages_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void OnClicked(object sender, EventArgs args)
        {
            this.Navigation.PushAsync(new HomeView());
        }
    }
}