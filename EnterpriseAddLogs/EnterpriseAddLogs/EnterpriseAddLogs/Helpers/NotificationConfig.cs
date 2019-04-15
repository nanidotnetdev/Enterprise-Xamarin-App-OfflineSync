using System.Drawing;
using Acr.UserDialogs;

namespace EnterpriseAddLogs.Helpers
{
    public class NotificationConfig
    {
        public static ToastConfig ErrorToast(string message)
        {
            return new ToastConfig(message).SetBackgroundColor(Color.Red);
        }

        public static ToastConfig SuccessToast(string message)
        {
            return new ToastConfig(message).SetBackgroundColor(Color.Green);
        }
    }
}
