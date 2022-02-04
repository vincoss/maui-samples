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
        private async void OnSelectKeyworkCommand(BoardCreateDto args)
        {
            if (args == null || IsBusy)
            {
                return;
            }

            var message = new PickerData<KeyDataGuidString>();

            if (Board.Folder != null)
            {
                message.ItemsSource = new[] { args.Folder };
            }

            await _navigator.ShellPushAsync($"{BoardConstants.FolderPickerRoute}");
            _navigator.Send<BoardCreateViewModel, PickerData<KeyDataGuidString>>(this, nameof(message.ItemsSource), message);

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
    }

    public class PickerData<T>
    {
        public bool IsSingleSelection { get; set; } = true;

        public IEnumerable<T> ItemsSource { get; set; } = Enumerable.Empty<T>();
    }


}