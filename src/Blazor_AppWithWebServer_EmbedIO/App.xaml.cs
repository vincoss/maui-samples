namespace Blazor_AppWithWebServer
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

            EmbedIO_ServerConsole.ServerHostingExtensions.Run(new string[0]);
        }
    }
}
