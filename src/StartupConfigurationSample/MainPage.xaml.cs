using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

using StartupConfigurationSample.Interfaces;
using System;

namespace StartupConfigurationSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            txtMessage.Text = "Loading...";

            var service = App.ServiceProvider.GetService<IHttpService>();
            var result = await service.GetAsync();

            txtMessage.Text = $"Status code: {result}";
        }
    }
}
