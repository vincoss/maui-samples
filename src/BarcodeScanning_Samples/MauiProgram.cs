﻿using Camera.MAUI;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using BarcodeScanning;


namespace BarcodeScanning_Samples
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseBarcodeReader()
                .UseBarcodeScanning()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).ConfigureMauiHandlers(handlers =>
                {
                    // Add the handlers
                    //handlers.AddBarcodeScannerHandler();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
