using MauiSharedLibrary.Dto;
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
            Title = "Select Keyboard";

            MessagingCenter.Subscribe<MainPageViewModel, PickerData<KeyDataGuidString>>(this, nameof(PickerData<KeyDataGuidString>.ItemsSource), async (obj, item) =>
            {
                var data = item;

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
                await LoadDaData();
                Select(GetSelection());
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        protected async override void OnOkCommand()
        {
            MessagingCenter.Send<KeywordPickerViewModel, PickerData<KeyDataGuidString>>(this, nameof(PickerData<KeyDataGuidString>.ItemsSource), null);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
