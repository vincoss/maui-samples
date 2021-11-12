using Android.App;
using Android.Runtime;
using AsyncCommandMvvm;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace Default_MauiApp
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