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
            secondWindow.Width = 300;
            secondWindow.Height = 300;
            secondWindow.X = 50;
            secondWindow.Y = 50;

            Application.Current.OpenWindow(secondWindow);
        }
    }
}