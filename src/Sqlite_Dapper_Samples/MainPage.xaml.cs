using ShortMvvm.ViewModels;
using Sqlite_Dapper_Samples.Entities.Model;
using Sqlite_Dapper_Samples.Interfaces;
using Sqlite_Dapper_Samples.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Sqlite_Dapper_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var viewModel = App.ServiceProvider.GetService<ProductListViewModel>();
            BindingContext = viewModel;
        }

    }

    public class ProductListViewModel : BaseViewModel
    {
        private readonly IItemService _itemService;
        private readonly IDatabaseService _databaseService;

        public ProductListViewModel(IItemService itemService, IDatabaseService databaseService)
        {
            _itemService = itemService;
            _databaseService = databaseService;

            ItemsSource = new ObservableCollection<Item>();

            RefreshCommand = new Command(OnRefreshCommand);
            AddCommand = new Command(OnAddCommand);
        }

        public async void Initialize()
        {
            ItemsSource.Clear();
            DatabasePath = _databaseService.ConnectionString;
            var products = await _itemService.Get();

            foreach (var p in products)
            {
                ItemsSource.Add(p);
            }
        }

        private void OnRefreshCommand()
        {
            Initialize();
        }

        private async void OnAddCommand()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                return;
            }

            var item = Item.Create(ProductName, DateTime.Now);
            item.JobId = 1;

            await _itemService.Create(item);
            ProductName = null;

            OnRefreshCommand();
        }

        public ObservableCollection<Item> ItemsSource { get; set; }

        public ICommand RefreshCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set { SetProperty(ref _productName, value); }
        }

        private string _databasePath;

        public string DatabasePath
        {
            get { return _databasePath; }
            set { SetProperty(ref _databasePath, value); }
        }
    }
}