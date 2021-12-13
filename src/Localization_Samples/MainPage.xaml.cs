using Localization_Samples.Localization;
using Localization_Samples_Library;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Localization_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            lblWelcome.Text = AppResources.Welcome;

            var text = string.Join(Environment.NewLine, CultureApp.Cache);
            editorCultures.Text = text;

            var manager = new ResourcesStringLocalizer();
            var value = manager[nameof(AppResources.Welcome)];

            lblTranslate.Text = value;
        }
    }
}
