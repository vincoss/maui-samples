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
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine($"Pixel width: {DeviceDisplay.Current.MainDisplayInfo.Width} / Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
            sb.AppendLine($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
            sb.AppendLine($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
            sb.AppendLine($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
            sb.AppendLine($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");

            var page = this.MainPage as MainPage;
            page.SetInfo(sb.ToString());
        }
    }
}