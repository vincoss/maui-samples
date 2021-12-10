using System;
using ToastNotification_Samples.Interface;


namespace ToastNotification_Samples.Platforms
{
    public class NotificationService : IToastMessage
    {
        public void LongAlert(string message)
        {
            throw new NotImplementedException();
        }

        public void ShortAlert(string message)
        {
            throw new NotImplementedException();
        }
    }
}
