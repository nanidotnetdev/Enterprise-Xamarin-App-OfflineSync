using System;
using System.Drawing;
using Acr.UserDialogs;

namespace EnterpriseAddLogs.Helpers
{
    public class Notifications
    {
        public static void ErrorToast(string message, TimeSpan? dismissTime = null)
        {
            UserDialogs.Instance.Toast(ErrorToastConfig(message, dismissTime));
        }

        private static ToastConfig ErrorToastConfig(string message, TimeSpan? dismissTime = null)
        {
            return new ToastConfig(message)
                .SetBackgroundColor(Color.Red)
                .SetMessageTextColor(Color.White)
                .SetDuration(dismissTime);
        }

        public static void SuccessToast(string message, TimeSpan? timeSpan = null)
        {
            UserDialogs.Instance.Toast(SuccessToastConfig(message, timeSpan));
        }

        private static ToastConfig SuccessToastConfig(string message, TimeSpan? timeSpan = null)
        {
            return new ToastConfig(message)
                .SetBackgroundColor(Color.Green)
                .SetMessageTextColor(Color.White);
        }

        public static void Toast(string title, TimeSpan? dismissTimer = null)
        {
            UserDialogs.Instance.Toast(title, dismissTimer);
        }

        public static void BusyIndicator(bool toggle = true, string title = null)
        {
            if(toggle)
                UserDialogs.Instance.ShowLoading(title);
            else
                UserDialogs.Instance.HideLoading();
        }

        public static void Alert(string message, string title)
        {
            UserDialogs.Instance.Alert(message, title);
        }

        public static void Alert(string message)
        {
            Alert(message, null);
        }
    }
}
