using OidcSamples.Views;

namespace OidcSamples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = 
                new HomeView();
                //new AppShell();
        }
    }
}