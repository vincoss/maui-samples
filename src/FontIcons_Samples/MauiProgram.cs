using Microsoft.Maui.Controls.Compatibility;


namespace FontIcons_Samples
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
                    fonts.AddFont("MaterialIconsSharp-Regular.otf", "MaterialDesign");
                    fonts.AddFont("Font-Awesome-5-Free-Solid-900.otf", "FontAwesome");
                });

            return builder.Build();
        }
    }
}