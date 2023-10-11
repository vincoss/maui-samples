using ShortMvvm.ViewModels;

namespace Setup_Samples.Pages;

public partial class SetupPage : ContentPage
{
    private readonly SetupPageViewModel _model = new SetupPageViewModel();

    public SetupPage()
	{
		InitializeComponent();

        BindingContext = _model;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _model.InitializeAsync();
    }

    public class SetupPageViewModel : BaseViewModel
	{
        public override async Task InitializeAsync()
        {
            // NOTE: not to await, just show the UI
            await Task.Factory.StartNew(async () =>
              {
                  // Get services in here

                  // Sample progress

                  Action<double, string> progressAction = (v, m) =>
                  {
                      App.Current.Dispatcher.Dispatch(() =>
                      {
                          PercentLabel = $"{(int)(v * 100)} %";
                          MessageLabel = m;
                      });
                  };

                  progressAction(.55, "Test");

                  await Task.Delay(5000);

                  // Navigate after complete
                  App.Current.Dispatcher.Dispatch(async () =>
                  {
                      await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                      var page = App.Current.MainPage.Navigation.NavigationStack[0];
                      App.Current.MainPage.Navigation.RemovePage(page);
                  });
              });
        }

        private string _percent = string.Empty;
        public string PercentLabel
        {
            get { return _percent; }
            set { SetProperty(ref _percent, value); }
        }

        private string _messageLabel = string.Empty;
        public string MessageLabel
        {
            get { return _messageLabel; }
            set { SetProperty(ref _messageLabel, value); }
        }
    }
}