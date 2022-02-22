using ItemsPicker_Samples.Sevices;
using ShortMvvm.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsPicker_Samples.ViewModels
{
    public class FrameworkPickerViewModel : BasePickerViewModel
    {
        public FrameworkPickerViewModel() : base()
        {
            Title = "Select frameworks";

            MessagingCenter.Subscribe<MainPageViewModel, PickerData<KeyDataIntString>>(this, nameof(PickerData<KeyDataIntString>.ItemsSource), async (obj, item) =>
            {
                var data = item.ItemsSource;
                IsSingleSelection = item.IsSingleSelection;
                Select(data);
            });

            /* 
             
                +must load all items
                +must pass current selected items
                must return selected items
                must sort selected items to top
            */
        }

        public override void Initialize()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                LoadDaData();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LoadDaData()
        {
            var query = TestData.Platforms.AsQueryable();

            if (string.IsNullOrWhiteSpace(_search) == false)
            {
                query = query.Where(x => x.Value.StartsWith(_search, StringComparison.OrdinalIgnoreCase));
            }

            Func<KeyDataIntString, bool> check = (x) =>
            {
                return ItemsSource.Any(o => o.Key == x.Key && o.IsSelected);
            };

            var items = query.ToList();
            var models = Map(items, check);

            ItemsSource.Clear();
            foreach (var m in models)
            {
                ItemsSource.Add(m);
            }

            OnPropertyChanged(nameof(ItemsSource));
        }

        protected async override void OnOkCommand()
        {
            var message = new PickerData<KeyDataIntString>();
            message.ItemsSource = MapSelected(ItemsSource);

            MessagingCenter.Send<FrameworkPickerViewModel, PickerData<KeyDataIntString>>(this, nameof(PickerData<KeyDataIntString>.ItemsSource), message);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
