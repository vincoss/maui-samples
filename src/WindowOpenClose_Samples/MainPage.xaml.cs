using WindowOpenClose_Samples.Pages;

namespace WindowOpenClose_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Window secondWindow = new Window(new PageOne());

            // Size and position
            secondWindow.Width = 300;
            secondWindow.Height = 300;
            secondWindow.X = 50;
            secondWindow.Y = 50;

            // OR
            // Get display size
            //var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

            //// Center the window
            //Window.X = (displayInfo.Width / displayInfo.Density - Window.Width) / 2;
            //Window.Y = (displayInfo.Height / displayInfo.Density - Window.Height) / 2;

            Application.Current.OpenWindow(secondWindow);
        }
    }
}