using CollectionView_Samples.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Collections.Generic;

namespace CollectionView_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var pages = new List<PageInfo>();
            pages.Add(new PageInfo { Type = typeof(CollectionViewAddView) });

            ListOfPages.ItemsSource = pages;
        }

        private async void ListOfPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var info = (PageInfo)e.CurrentSelection[0];
                var page = (Page)Activator.CreateInstance(info.Type);

                await this.Navigation.PushAsync(page);
            }
            ListOfPages.SelectedItem = null;
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
                    return base.GetType().ToString();
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
}
