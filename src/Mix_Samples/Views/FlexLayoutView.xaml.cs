using Microsoft.Maui.Controls;

using System;

namespace Mix_Samples.Views
{
    public partial class FlexLayoutView : ContentPage
    {
        public FlexLayoutView()
        {
            InitializeComponent();

            BindingContext = new StringFormatViewModel();
        }
    }

    public class FlexLayoutViewModel
    {
    }

}
