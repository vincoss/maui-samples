using Android.App;
using Android.Widget;
using System;
using ToastNotification_Samples.Interface;
using Application = Android.App.Application;

namespace ToastNotification_Samples.Platforms
{
    public class NotificationService : IToastMessage
    {
        public void ShortAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        public void LongAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}
