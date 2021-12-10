using Foundation;
using System;
using ToastNotification_Samples.Interface;
using UIKit;


namespace ToastNotification_Samples.Platforms
{
    public class NotificationService : IToastMessage
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void ShortAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            MesajBas(message, SHORT_DELAY);
        }

        public void LongAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            MesajBas(message, LONG_DELAY);
        }

        void MesajBas(string mesaj, double sure)
        {
            alertDelay = NSTimer.CreateRepeatingScheduledTimer(sure, (obj) =>
            {
                DismissCallback();
            });
            alert = UIAlertController.Create(null, mesaj, UIAlertControllerStyle.Alert);

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        private void DismissCallback()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);

            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}
