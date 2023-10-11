namespace MauiApp1;

public partial class PermissionsAndroidWebViewView : ContentPage
{
	public PermissionsAndroidWebViewView()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RequestCameraPermission();
    }

    private async Task RequestCameraPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.Camera>();
    }
}