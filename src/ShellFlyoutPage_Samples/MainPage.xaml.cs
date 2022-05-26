using Microsoft.Maui.Controls;

using ShellFlyoutPage_Samples.Views;
using System;

namespace ShellFlyoutPage_Samples
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();

            Routing.RegisterRoute("settings", typeof(SettingsView));
            Routing.RegisterRoute("settingsOne", typeof(SettingsOneView));
            Routing.RegisterRoute("settingsTwo", typeof(SettingsTwoView));

            this.Navigating += MainPage_Navigating;
            this.Navigated += MainPage_Navigated;
        }

        private void MainPage_Navigating(object sender, ShellNavigatingEventArgs e)
        {
        }

        private void MainPage_Navigated(object sender, ShellNavigatedEventArgs e)
        {
        }


        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var id = 1;
            await Shell.Current.GoToAsync($"settings?name={id}");
        }
    }
}
