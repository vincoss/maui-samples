﻿using MauiSharedLibrary.ViewModels;
using System.Windows.Input;

namespace MessagingCenter_Samples.Views
{
    public partial class CreateProductView : ContentPage
    {
        public CreateProductView()
        {
            InitializeComponent();

            var model = new CreateProductViewModel();
            BindingContext = model;
        }
    }

    public class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            CancelCommand = new Command(OnCancelCommand);

            MessagingCenter.Subscribe<MainPageViewModel, string>(this, "Message", async (obj, item) =>
            {

            });
        }

        private void OnCancelCommand()
        {
            MessagingCenter.Send(this, "Message", "Update");
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        public ICommand CancelCommand { get; private set; }

    }


}