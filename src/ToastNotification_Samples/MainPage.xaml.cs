using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using ToastNotification_Samples.Interface;
using ToastNotification_Samples.Platforms;


namespace ToastNotification_Samples
{
    public partial class MainPage : ContentPage
    {
        private readonly IToastMessage _toastMessage = new NotificationService();

        public MainPage()
        {
            InitializeComponent();
        }

        private void ShortToastClickerClicked(object sender, EventArgs e)
        {
            _toastMessage.ShortAlert(nameof(ShortToastClickerClicked));
        }

        private void LongToastClickerClicked(object sender, EventArgs e)
        {
            _toastMessage.LongAlert(nameof(LongToastClickerClicked));
        }
    }
}
