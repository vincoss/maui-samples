using Microsoft.Extensions.DependencyInjection;
using Setup_Samples.Pages;


namespace Setup_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

           // MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            ContentPage page;

            if (true) // NOTE: here check for setup completion
            {
                page = new SetupPage();
            }
            else
            {
                page = new MainPage();
            }

            return new Window(new NavigationPage(page)) { Title = "Setup_Samples" };
        }
    }
}