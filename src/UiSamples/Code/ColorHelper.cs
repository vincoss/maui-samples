using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiSamples.Code
{
    public class ColorHelper
    { 
        /// <summary>
        /// https://htmlcolorcodes.com/
        /// </summary>
        public static IDictionary<string, string> _common = new Dictionary<string, string>
        {
            { "White", "#FFFFFF" },
            { "Silver", "#C0C0C0" },
            { "Gray", "#808080" },
            { "Black", "#000000" },
            { "Red", "#FF0000" },
            { "Maroon", "#800000" },
            { "Yellow", "#FFFF00" },
            { "Olive", "#808000" },
            { "Lime", "#00FF00" },
            { "Green", "#008000" },
            { "Aqua", "#00FFFF" },
            { "Teal", "#008080" },
            { "Blue", "#0000FF" },
            { "Navy", "#000080" },
            { "Fuchsia", "#FF00FF" },
            { "Purple", "#800080" }
        };

        public IEnumerable<KeyValuePair<string, string>> Get()
        {
            throw new NotImplementedException();
            //return _common.OrderBy(c => Color.FromArgb(Convert.ToInt32(c.Value.Substring(1), 16)).GetBrightness())
            //    return _common.OrderBy(c => Color.FromArgb(c.Value).GetLuminosity())
        }
    }
}