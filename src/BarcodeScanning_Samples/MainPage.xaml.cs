using BarcodeScanning_Samples.Pages;

namespace BarcodeScanning_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OneButton_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new BarcodeOnePage());
        }

        private void TwoButton_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new BarcodeTwoPage());
        }

        private void ThreeButton_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new BarcodeThreePage());
        }
    }

}
