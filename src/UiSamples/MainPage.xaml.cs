using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using UiSamples.Views;

namespace UiSamples
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;
            this.Appearing += MainPage_Appearing;
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            _viewModel.Initialize();
        }

        private async void ListOfPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cv = sender as CollectionView;

            if (e.CurrentSelection != null && e.CurrentSelection.Any())
            {
                var info = (PageInfo)e.CurrentSelection.First();
                var page = (Page)Activator.CreateInstance(info.Type);

                await this.Navigation.PushAsync(page);
            }

            cv.SelectedItem = null;
        }
    }

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            ItemsSource = new ObservableCollection<PageInfo>();
        }

        public override void Initialize()
        {
            ItemsSource.Add(new PageInfo { Type = typeof(UI_CollectionView) });
            ItemsSource.Add(new PageInfo { Type = typeof(UI_TableView) });
        }

        public ObservableCollection<PageInfo> ItemsSource { get; }
    }

    public class PageInfo
    {
        public Type Type { get; set; }
        public string Name
        {

            get
            {
                if (Type != null)
                {
                    return Type.Name;
                }
                return base.ToString();
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return base.ToString();
            }
            return Name;
        }
    }
}