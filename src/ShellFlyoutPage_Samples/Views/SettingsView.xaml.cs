using Microsoft.Maui.Controls;

using System;

namespace ShellFlyoutPage_Samples.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"..?parameterToPassBack={Guid.NewGuid()}");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("settingsOne");
        }
    }
}
