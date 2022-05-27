using AsyncAwaitBestPractices;
using AsyncCommandMvvm.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;

namespace AsyncCommandMvvm
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            var model = new SampleViewModel();
            model.ErrorOccurred += HandleErrorOccurred;
            BindingContext = model;

            model.InitializeAsync().SafeFireAndForget();
        }
        private async void HandleErrorOccurred(object sender, string e)
        {
            await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", e, "OK"));
        }
    }
}
