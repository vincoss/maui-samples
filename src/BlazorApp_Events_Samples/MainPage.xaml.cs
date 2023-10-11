namespace BlazorApp_Events_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += MainPage_Loaded;
            this.Unloaded += MainPage_Unloaded;
            this.Focused += MainPage_Focused;
            this.NavigatingFrom += MainPage_NavigatingFrom;
            this.NavigatedTo += MainPage_NavigatedTo;

            DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
        }

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
        }

        private void MainPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
        }
        private void MainPage_NavigatingFrom(object sender, NavigatingFromEventArgs e)
        {
        }

        private void MainPage_Focused(object sender, FocusEventArgs e)
        {
        }

        private void MainPage_Unloaded(object sender, EventArgs e)
        {
        }

        private void MainPage_Loaded(object sender, EventArgs e)
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}