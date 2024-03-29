﻿using MauiApp1;
using Mix_Samples.Pages;
using Mix_Samples.Views;

namespace Mix_Samples
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
            pages.Add(new PageInfo { Type = typeof(NavigationPageTitleView) });
            pages.Add(new PageInfo { Type = typeof(RadioButtonView) });
            pages.Add(new PageInfo { Type = typeof(UserView) });
            pages.Add(new PageInfo { Type = typeof(StringFormatView) });
            pages.Add(new PageInfo { Type = typeof(ActivityIndicatorView) });
            pages.Add(new PageInfo { Type = typeof(FlexLayoutView) });
            pages.Add(new PageInfo { Type = typeof(ListAllFilesInAppInstallPathView) });
            pages.Add(new PageInfo { Type = typeof(ConsentPage) });

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