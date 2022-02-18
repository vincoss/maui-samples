using ItemsPicker_Samples.Sevices;
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
            model.Initialize();
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
            Keyword = TestData.Keywords.FirstOrDefault();
        }

        private async void OnSelectKeyworkCommand(KeyDataIntString args)
        {
            if (IsBusy)
            {
                return;
            }

            var message = new PickerData<KeyDataIntString>();
            message.IsSingleSelection = false;
            message.ItemsSource = args != null ? new[] { args } : Enumerable.Empty<KeyDataIntString>();

            var page = new PickerListView();
            var model = new KeywordPickerViewModel();
            page.BindingContext = model;
            model.Initialize();

            await App.Current.MainPage.Navigation.PushAsync(page);

            MessagingCenter.Send(this, nameof(message.ItemsSource), message);
        }

        public ICommand SelectKeywordCommand { get; private set; }

        private KeyDataIntString _keyword;

        public KeyDataIntString Keyword
        {
            get { return _keyword; }
            set { SetProperty(ref _keyword, value); }
        }
    }

    public class PickerData<T>
    {
        public bool IsSingleSelection { get; set; } = true;

        public IEnumerable<T> ItemsSource { get; set; } = Enumerable.Empty<T>();
    }
}