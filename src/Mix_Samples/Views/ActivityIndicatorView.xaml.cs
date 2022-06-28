using Microsoft.Maui.Controls;

using System;

namespace Mix_Samples.Views
{
    public partial class ActivityIndicatorView : ContentPage
    {
        public ActivityIndicatorView()
        {
            InitializeComponent();

            BindingContext = new ActivityIndicatorViewModel();
        }
    }

    public class ActivityIndicatorViewModel
    {
        public bool IsBusy { get; set; } = true;
    }

}
