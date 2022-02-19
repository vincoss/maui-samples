using ShortMvvm.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RefreshView_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new RefreshViewSampleViewModel();
        }

    }
    public class RefreshViewSampleViewModel : BaseViewModel
    {
        public RefreshViewSampleViewModel()
        {
            RefreshCommand = new Command(OnRefreshCommand);
            LoadItems();
        }

        private async void LoadItems()
        {
            try
            {
                IsBusy = true;
                Items = Enumerable.Range(0, 100);
                LastRefresh = DateTime.Now;
                await Task.Delay(1000);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnRefreshCommand()
        {
            LoadItems();
        }

        public IEnumerable<int> Items { get; private set; }

        private DateTime? _lastRefresh;
        public DateTime? LastRefresh
        {
            get { return _lastRefresh; }
            set { SetProperty(ref _lastRefresh, value); }
        }

        public ICommand RefreshCommand { get; private set; }
    }
}