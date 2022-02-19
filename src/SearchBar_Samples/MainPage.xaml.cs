using ShortMvvm;
using ShortMvvm.ViewModels;
using System.Windows.Input;

namespace SearchBar_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel();
        }

    }

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SearchResults = TempDataService.Fruits;
        }

        public ICommand PerformSearchCommand => new Command<string>((string query) =>
        {
            SearchResults = TempDataService.GetSearchResults(query);
        });

        private IEnumerable<string> _searchResults;

        public IEnumerable<string> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }
    }
}