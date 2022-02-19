using ShortMvvm.ViewModels;
using System.Windows.Input;

namespace MessagingCenter_Samples.Views
{
    public partial class CreateProductView : ContentPage
    {
        public CreateProductView()
        {
            InitializeComponent();

            var model = new CreateProductViewModel();
            BindingContext = model;
        }
    }

    public class CreateProductViewModel : BaseViewModel
    {
        public CreateProductViewModel()
        {
            CancelCommand = new Command(OnCancelCommand);

            MessagingCenter.Subscribe<MainPageViewModel, string>(this, nameof(Message), async (obj, item) =>
            {
                Message = item;
            });
        }

        private void OnCancelCommand()
        {
            MessagingCenter.Send(this, nameof(Message), Message);
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        public ICommand CancelCommand { get; private set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }


}