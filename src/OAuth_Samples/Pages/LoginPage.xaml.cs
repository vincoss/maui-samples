using OAuth_Samples.ViewModels;

namespace OAuth_Samples.Pages;

public partial class LoginPage : ContentPage
{
	private readonly LoginPageViewModel _model;
    private bool _isFirstTime = true;

    public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();


        _model = viewModel;
        BindingContext = _model;

        //Action act = async () =>
        //{
        //    await _model.InitializeAsync();
        //};

        //act();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isFirstTime)
        {
            _isFirstTime = false;
            await _model.InitializeAsync();
        }
    }
}