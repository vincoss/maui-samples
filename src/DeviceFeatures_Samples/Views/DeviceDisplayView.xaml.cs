namespace DeviceFeatures_Samples.Views;

public partial class DeviceDisplayView : ContentPage
{
	public DeviceDisplayView()
	{
		InitializeComponent();

        DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;

        ReadDeviceDisplay();
    }

    private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        ReadDeviceDisplay();
    }

    private void ReadDeviceDisplay()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"Pixel width: {DeviceDisplay.Current.MainDisplayInfo.Width} / Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
        sb.AppendLine($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
        sb.AppendLine($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
        sb.AppendLine($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
        sb.AppendLine($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");

        lblDisplayInfo.Text = sb.ToString();
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        DeviceDisplay.Current.KeepScreenOn = e.Value;
    }
}