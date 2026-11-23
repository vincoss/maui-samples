using Blazor_AppWithWebServer_EmbedIO.Services;

namespace Blazor_AppWithWebServer_EmbedIO
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();

            ServiceProvider = _serviceProvider;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            ContentPage page = new MainPage();

            return new Window(new NavigationPage(page)) { Title = "EmbedIO Web Server" };
        }

        protected override async void OnStart()
        {
            base.OnStart();

            var startup = _serviceProvider.GetService<IStartup>();
            await startup.RunAsync();
        }

        public static IServiceProvider ServiceProvider { get; set; }
    }
}
