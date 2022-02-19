using ShortMvvm.ViewModels;
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
    }

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            CreateCommand = new Command(OnCreateCommand);

            MessagingCenter.Subscribe<CreateProductViewModel, string>(this, nameof(Message), async (obj, item) =>
            {
                Message = item;
            });
        }

        private void OnCreateCommand()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new CreateProductView());

            // NOTE: Must push after navigation to ensure that view model subscribled to the message.
            MessagingCenter.Send(this, nameof(Message), Message);
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