using System.Windows.Input;

namespace Mix_Samples.Pages;

public partial class ConsentPage : ContentPage
{
    public ICommand TapCommand => new Command<string>(async (url) =>
    {
        await Launcher.OpenAsync(url);
    });

    public ConsentPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
}