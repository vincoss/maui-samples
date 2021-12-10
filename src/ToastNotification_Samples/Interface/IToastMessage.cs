using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToastNotification_Samples.Interface
{
    public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
