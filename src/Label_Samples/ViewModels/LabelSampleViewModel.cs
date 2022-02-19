using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Label_Samples.ViewModels
{
    public class LabelSampleViewModel
    {
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Launcher.OpenAsync(new System.Uri(url));
        });

        public string FormatedLabelString
        {
            get { return "Jobs"; }
        }

        public string FirstName
        {
            get { return "Ferdinand"; }
        }
    }
}
