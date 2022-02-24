namespace BlazorVueJS_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if(ex == null)
            {
                return;
            }

            Console.WriteLine(ex);
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new MainPage() { Title = "BlazorVueJS_Samples" });
        }
    }
}