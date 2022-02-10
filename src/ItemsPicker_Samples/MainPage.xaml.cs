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
        }
    }

    public class MainPageViewModel : BaseViewModel
    {
        private async void OnSelectKeyworkCommand(KeyDataGuidString args)
        {
            if (args == null || IsBusy)
            {
                return;
            }

            var message = new PickerData<KeyDataGuidString>();
            message.ItemsSource = new[] { args };

            var page = new PickerListView();
            page.BindingContext = new KeywordPickerViewModel();
            await App.Current.MainPage.Navigation.PushAsync(page);

            MessagingCenter.Send(this, nameof(message.ItemsSource), message);

            // await _navigator.PushAsync(_navigator.GetView(nameof(PickerListViewModel)));
            //await _navigator.PushAsync<PickerListViewModel>(typeof(PickerListView), m =>
            //{
            //    m.Title = AppResources.SelectCompany;
            //    m.DataLoaderType = PickerType.Company;
            //    m.IsSingleSelection = true;
            //    m.GetSelection = CompanyGetSelection;
            //    m.SetSelection = CompanySetSelection;
            //});
        }

        public ICommand SelectFolderCommand { get; private set; }

        public KeyDataGuidString Keyword { get; set; }
    }

    public class PickerData<T>
    {
        public bool IsSingleSelection { get; set; } = true;

        public IEnumerable<T> ItemsSource { get; set; } = Enumerable.Empty<T>();
    }


}