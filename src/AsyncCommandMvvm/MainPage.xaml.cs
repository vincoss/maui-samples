using AsyncCommandMvvm.ViewModels;
using AsyncAwaitBestPractices;
using AsyncCommandMvvm.ViewModels;


namespace AsyncCommandMvvm
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var model = new SampleViewModel();
            model.ErrorOccurred += HandleErrorOccurred;
            BindingContext = model;

            model.InitializeAsync().SafeFireAndForget();
        }
        private async void HandleErrorOccurred(object sender, string e)
        {
            await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", e, "OK"));
        }
    }
}