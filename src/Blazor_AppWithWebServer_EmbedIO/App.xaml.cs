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

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            base.OnStart();

            var server = _serviceProvider.GetService<IEmbedServer>();
            server.Run(new string[0]);
        }
    }
}
