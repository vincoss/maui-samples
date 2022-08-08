namespace OAuth_Samples
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}