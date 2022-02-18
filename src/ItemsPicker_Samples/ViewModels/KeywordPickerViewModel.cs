using ItemsPicker_Samples.Sevices;
using ShortMvvm.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsPicker_Samples.ViewModels
{
    public class KeywordPickerViewModel : BasePickerViewModel
    {
        public KeywordPickerViewModel()
        {
            Title = "Select Keyword";

            MessagingCenter.Subscribe<MainPageViewModel, PickerData<KeyDataIntString>>(this, nameof(PickerData<KeyDataIntString>.ItemsSource), async (obj, item) =>
            {
                var data = item.ItemsSource;
                IsSingleSelection = item.IsSingleSelection;
                Select(data);
            });

            /* 
             
                must load all items
                must pass current selected items
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
                ItemsSource.Clear();
                LoadDaData();
                //Select(GetSelection());
            }
            finally
            {
                IsBusy = false;
              //  IsRefreshing = false;
            }
        }

        private void LoadDaData()
        {
            var items = TestData.Keywords;
            var models = Map(items, false);
           
            foreach(var m in models)
            {
                ItemsSource.Add(m);
            }

            OnPropertyChanged(nameof(ItemsSource));
        }

        protected async override void OnOkCommand()
        {
            var message = new PickerData<KeyDataIntString>();
            message.ItemsSource = MapSelected(ItemsSource);

            MessagingCenter.Send<KeywordPickerViewModel, PickerData<KeyDataIntString>>(this, nameof(PickerData<KeyDataIntString>.ItemsSource), message);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

 

    }
}
