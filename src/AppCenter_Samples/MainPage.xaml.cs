﻿using AppCenter_ConfigurationSample.Services;

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