using OAuth_Samples.ViewModels;

namespace OAuth_Samples.Pages;

public partial class LoginPage : ContentPage
{
	private readonly LoginPageViewModel _model;

    public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();

		_model = viewModel;
		BindingContext = _model;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await _model.InitializeAsync();
	}
}