using ViewModelIoc_Samples.Views;

namespace ViewModelIoc_Samples
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new HomeView())) { Title = "ViewModelIocSample" };
        }

        public static IServiceProvider ServiceProvider { get; private set; }
    }
}