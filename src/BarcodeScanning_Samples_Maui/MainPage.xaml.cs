﻿namespace BarcodeScanning_Samples_Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            cameraView.BarCodeOptions = new()
            { 
                ReadMultipleCodes = true,
                AutoRotate = true,
                PossibleFormats =
                {
                    ZXing.BarcodeFormat.QR_CODE,
                    ZXing.BarcodeFormat.CODE_39,
                    ZXing.BarcodeFormat.All_1D,
                }
            };

        }



        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.Cameras.Count > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                });
            }
        }

        private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
            });
        }
    }

}