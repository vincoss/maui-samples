using MauiSharedLibrary.ViewModels;
using MessagingCenter_Samples.Views;
using System.Windows.Input;

namespace MessagingCenter_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var model = new MainPageViewModel();
            BindingContext = model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            CreateCommand = new Command(OnCreateCommand);

            MessagingCenter.Subscribe<CreateProductViewModel>(this, nameof(Message), (m) =>
            {
            });
        }

        private void OnCreateCommand()
        {
            MessagingCenter.Send<MainPageViewModel, string>(this, "Message", Message);
            App.Current.MainPage.Navigation.PushModalAsync(new CreateProductView());
        }

        public ICommand CreateCommand { get; private set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }
}