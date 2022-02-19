using Microsoft.Maui.Graphics;

namespace MauiGraphics_Samples
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
                });

          

          //  builder.ConfigureGraphicsControls(Microsoft.Maui.Graphics.Controls.DrawableType.Material)

            return builder.Build();
        }
    }
}