using ItemsPicker_Samples.ViewModels;
using ItemsPicker_Samples.Views;
using MauiSharedLibrary.Dto;
using MauiSharedLibrary.ViewModels;
using System.Windows.Input;

namespace ItemsPicker_Samples
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
            SelectKeywordCommand = new Command<KeyDataIntString>(OnSelectKeyworkCommand);
        }

        public override void Initialize()
        {
            // TODO: select values here 
        }

        private async void OnSelectKeyworkCommand(KeyDataIntString args)
        {
            if (IsBusy)
            {
                return;
            }

            var message = new PickerData<KeyDataIntString>();
            message.ItemsSource = args != null ? new[] { args } : Enumerable.Empty<KeyDataIntString>();

            var page = new PickerListView();
            //var model = new KeywordPickerViewModel();
            //page.BindingContext = model;
            //model.Initialize();

            await App.Current.MainPage.Navigation.PushAsync(page);

            MessagingCenter.Send(this, nameof(message.ItemsSource), message);
        }

        public ICommand SelectKeywordCommand { get; private set; }

        public KeyDataIntString Keyword { get; set; }
    }

    public class PickerData<T>
    {
        public bool IsSingleSelection { get; set; } = true;

        public IEnumerable<T> ItemsSource { get; set; } = Enumerable.Empty<T>();
    }
}