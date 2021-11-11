﻿using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using System;
using Microsoft.Maui.Hosting;


namespace Sqlite_EF_Samples
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}