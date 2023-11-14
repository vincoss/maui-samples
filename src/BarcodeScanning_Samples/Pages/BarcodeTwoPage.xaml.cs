using ZXing.Net.Maui;

namespace BarcodeScanning_Samples.Pages;

public partial class BarcodeTwoPage : ContentPage
{
	public BarcodeTwoPage()
	{
		InitializeComponent();

        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = true
        };

        cameraBarcodeReaderView.IsTorchOn = !cameraBarcodeReaderView.IsTorchOn;
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                barcodeResult.Text = barcode.Value;
            } );
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
        }
    }
}