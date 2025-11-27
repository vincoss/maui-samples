using Default_MauiApp.Models;
using Default_MauiApp.PageModels;

namespace Default_MauiApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}