namespace WindowOpenClose_Samples.Pages;

public partial class PageOne : ContentPage
{
	public PageOne()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var windows = Application.Current.Windows;

        Application.Current.CloseWindow(this.Window);
    }
}