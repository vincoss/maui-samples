using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoneword.Xaml
{
    public class GlobalFontSizeExtension : IMarkupExtension
    {
        public const double MyFontSize = 28;

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return MyFontSize;
        }
    }

    static class SharedResources
    {
        public static readonly Color FontColor = Color.FromRgb(0, 0, 0xFF);
        public static readonly Color BackgroundColor = Color.FromRgb(0xFF, 0xF0, 0xAD);
    }
}
