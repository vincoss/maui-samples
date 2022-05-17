using Microsoft.Maui.Controls;

using System;

namespace ShellFlyoutPage_Samples.Views
{
    public partial class SettingsTwoView : ContentPage
    {
        public SettingsTwoView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("../../");
        }
    }
}
