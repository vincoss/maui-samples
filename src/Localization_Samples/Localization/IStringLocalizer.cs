using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization_Samples.Localization
{
    public interface IStringLocalizer
    {
        string this[string name] { get; }
    }
}
