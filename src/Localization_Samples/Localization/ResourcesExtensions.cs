using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System;
using System.Resources;
using System.Collections;

namespace Localization_Samples.Localization
{
    public class ResourcesExtensions
    {
        public static IEnumerable<string> GetAllKeys()
        {
            var id = typeof(AppResources).FullName;
            var manager = new ResourceManager(id, typeof(ResourcesExtensions).Assembly);
            var set = manager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            var resourceStrings = new List<string>();
            var enumerator = set.GetEnumerator();
            while (enumerator.MoveNext())
            {
                resourceStrings.Add(enumerator.Key.ToString());
            }
            return resourceStrings;
        }
    }
}
