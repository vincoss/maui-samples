using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace FlyoutPage_Samples
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();

            flyoutPage.listView.SelectionChanged += ListView_SelectionChanged;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection == null || e.CurrentSelection.Count <= 0)
            {
                return;
            }

            var item = e.CurrentSelection[0] as FlyoutPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                flyoutPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
