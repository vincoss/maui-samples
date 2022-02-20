using ItemsPicker_Samples.Sevices;
using ItemsPicker_Samples.ViewModels;
using ItemsPicker_Samples.Views;
using ShortMvvm.Dto;
using ShortMvvm.ViewModels;
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
            MessagingCenter.Subscribe<KeywordPickerViewModel, PickerData<KeyDataIntString>>(this, nameof(PickerData<KeyDataIntString>.ItemsSource), async (obj, item) =>
            {
                var data = item.ItemsSource; 
                Keyword = data.FirstOrDefault();
            });

            SelectKeywordCommand = new Command<KeyDataIntString>(OnSelectKeyworkCommand);
        }

        public override void Initialize()
        {
            Keyword = TestData.Keywords.Skip(3).FirstOrDefault();
        }

        private async void OnSelectKeyworkCommand(KeyDataIntString args)
        {
            if (IsBusy)
            {
                return;
            }

            var message = new PickerData<KeyDataIntString>();
            message.IsSingleSelection = true;
            message.ItemsSource = args != null ? new[] { args } : Enumerable.Empty<KeyDataIntString>();

            var page = new PickerListView();
            var model = new KeywordPickerViewModel();
            page.BindingContext = model;
            model.Initialize();

            await App.Current.MainPage.Navigation.PushModalAsync(page);

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