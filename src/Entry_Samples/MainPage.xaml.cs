using Entry_Samples.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Entry_Samples
{
    public partial class MainPage : ContentPage
    {          
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonNew_Clicked(object sender, EventArgs e)
        {
            var view = new EntryFocusView();
            var model = new EntryFocusViewModel();
            model.IsNew = true;
            view.BindingContext = model;
            this.Navigation.PushAsync(view);
        }

        private void buttonEdit_Clicked(object sender, EventArgs e)
        {
            var view = new EntryFocusView();
            var model = new EntryFocusViewModel();
            model.A = "A";
            view.BindingContext = model;

            this.Navigation.PushAsync(view);
        }
    }
}
