using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Geolocation_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            lblInfo.Text = "Loading...";

            var result = await App.GeoService.Get();
            var last = await App.GeoService.Get2();

            lblInfo.Text = result.ToString();
            lblInfoLast.Text = last.ToString();
        }
    }
}
