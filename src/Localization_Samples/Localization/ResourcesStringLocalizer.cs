using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using Localization_Samples_Library;

namespace Localization_Samples.Localization
{
    public class ResourcesStringLocalizer : IStringLocalizer
    {
        private static readonly string ResourceId = typeof(AppResources).FullName;
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
        () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(AppResources)).Assembly));

        private string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var translation = ResMgr.Value.GetString(key);
            if (translation == null)
            {
                throw new ArgumentException($"Key '{key}' was not found in resources '{ResourceId}' for culture '{CultureInfo.CurrentCulture}'.");
            }
            return translation;
        }

        public string this[string name]
        {
            get { return Get(name); }
        }

    }
}
