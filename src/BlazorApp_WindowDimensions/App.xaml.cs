namespace BlazorApp_WindowDimensions
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            MainPage = new MainPage();
        }

        public override void OpenWindow(Window window)
        {
            base.OpenWindow(window);

            DeviceDisplay.MainDisplayInfoChanged += OnDisplayInfoChangedEventArgs;
        }

        private void OnDisplayInfoChangedEventArgs(object sender, DisplayInfoChangedEventArgs args)
        {

        }
    }
}