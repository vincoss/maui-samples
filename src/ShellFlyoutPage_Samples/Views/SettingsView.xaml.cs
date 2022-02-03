using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace ShellFlyoutPage_Samples.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"..?parameterToPassBack={Guid.NewGuid()}");
        }
    }
}
