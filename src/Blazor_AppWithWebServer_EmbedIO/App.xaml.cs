namespace Blazor_AppWithWebServer_EmbedIO
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            Blazor_AppWithWebServer_EmbedIO.Services.ServerHostingExtensions.Run(new string[0]);
        }
    }
}
