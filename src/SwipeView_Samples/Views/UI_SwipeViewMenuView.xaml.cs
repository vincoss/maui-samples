using Microsoft.Maui;
using Microsoft.Maui.Controls;

using System;

namespace SwipeView_Samples.Views
{
    public partial class UI_SwipeViewMenuView : ContentPage
    {
        public UI_SwipeViewMenuView()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            swipeView.Open(OpenSwipeItem.RightItems);
        }
    }
}