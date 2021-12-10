using AppCenter_ConfigurationSample.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace AppCenter_Samples
{
    public partial class MainPage : ContentPage
    {
        public IUpdateService UpdateService { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

    }
}
