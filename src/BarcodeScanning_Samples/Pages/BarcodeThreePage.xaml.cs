using BarcodeScanning;
using ZXing.Net.Maui;


namespace BarcodeScanning_Samples.Pages
{

    public partial class BarcodeThreePage : ContentPage
    {
        public BarcodeThreePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await Methods.AskForRequiredPermissionAsync();
            base.OnAppearing();

            Barcode.CameraEnabled = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Barcode.CameraEnabled = false;
        }

        private void ContentPage_Unloaded(object sender, EventArgs e)
        {
            Barcode.Handler?.DisconnectHandler();
        }

        private void CameraView_OnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
        {
            if (e.BarcodeResults.Length > 0)
            {
                barcodeResult.Text = e.BarcodeResults[0].RawValue;
            }
        }
    }
}