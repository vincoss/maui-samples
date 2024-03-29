﻿namespace FontIcons_Samples
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIconsSharp-Regular.otf", "MaterialDesign");
                    fonts.AddFont("Font-Awesome-5-Free-Solid-900.otf", "FontAwesome");
                    fonts.AddFont("Sea-Life.otf", "SeaLife");
                });

            return builder.Build();
        }
    }
}