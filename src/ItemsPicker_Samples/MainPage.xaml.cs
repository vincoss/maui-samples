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
                Frameworks = new SelectedItemsDisplay<KeyDataIntString>
                {
                    ItemsSource = item.ItemsSource
                };
            });

            SelectFrameworksCommand = new Command<SelectedItemsDisplay<KeyDataIntString>>(OnSelectFrameworksCommand);
        }

        public override void Initialize()
        {
            Frameworks = new SelectedItemsDisplay<KeyDataIntString>
            {
                ItemsSource = new[]
                {
                    TestData.Keywords.Skip(3).FirstOrDefault()
                }
            };
        }

        private async void OnSelectFrameworksCommand(SelectedItemsDisplay<KeyDataIntString> args)
        {
            if (IsBusy)
            {
                return;
            }

            var message = new PickerData<KeyDataIntString>();
            message.IsSingleSelection = IsSingleSelection;
            message.ItemsSource = args != null ? args.ItemsSource : Enumerable.Empty<KeyDataIntString>();

            var page = new PickerListView();
            var model = new KeywordPickerViewModel();
            page.BindingContext = model;
            model.Initialize();

            await App.Current.MainPage.Navigation.PushModalAsync(page);

            MessagingCenter.Send(this, nameof(message.ItemsSource), message);
        }

        public ICommand SelectFrameworksCommand { get; private set; }

        private SelectedItemsDisplay<KeyDataIntString> _frameworks;

        public SelectedItemsDisplay<KeyDataIntString> Frameworks
        {
            get { return _frameworks; }
            set { SetProperty(ref _frameworks, value); }
        }

        private bool _isSingleSelection;

        public bool IsSingleSelection
        {
            get { return _isSingleSelection; }
            set { SetProperty(ref _isSingleSelection, value); }
        }
    }

    public class PickerData<T>
    {
        public bool IsSingleSelection { get; set; } = true;

        public IEnumerable<T> ItemsSource { get; set; } = Enumerable.Empty<T>();
    }

    public class SelectedItemsDisplay<T> 
    {
        public IEnumerable<T> ItemsSource { get; set; } = Enumerable.Empty<T>();

        public override string ToString()
        {
            var separator = ',';
            return String.Join(separator, ItemsSource).Trim(new[] { separator });
        }
    }
}