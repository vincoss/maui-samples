using System;
using ToastNotification_Samples.Interface;
using Windows.UI.Notifications;

namespace ToastNotification_Samples.Platforms
{
    public class NotificationService : IToastMessage
    {
        public void ShortAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            var toast = Get(message);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public void LongAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            var toast = Get(message, DateTimeOffset.Now.AddSeconds(10));
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private ToastNotification Get(string message, DateTimeOffset? expire = null)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;

            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(message));

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = expire;
            return toast;
        }
    }
}
