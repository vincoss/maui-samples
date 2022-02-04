using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
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
    }
}
