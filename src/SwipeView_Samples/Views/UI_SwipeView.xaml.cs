using Microsoft.Maui.Controls;

using System;

namespace SwipeView_Samples.Views
{
    public partial class UI_SwipeView : ContentPage
    {
        public UI_SwipeView()
        {
            InitializeComponent();
        }

        private async void FavoriteSwipeItem_Invoked(object sender, EventArgs e)
        {
            await this.DisplayAlert("Favorite", "Favorite delete", "OK");
        }

        private async void DeleteSwipeItem_Invoked(object sender, EventArgs e)
        {
            await this.DisplayAlert("Delete", "Swipe delete", "OK");
        }
    }
}