using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.ViewModels
{
    public class SettingsPageViewModel : PageViewModel
    {
        private ObservableRangeCollection<Settings> _settings;

        public ObservableRangeCollection<Settings> Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        public SettingsPageViewModel(INavigator navigator) : base(navigator)
        {
            _settings = new ObservableRangeCollection<Settings>();

            SetOptions();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetOptions()
        {
            Settings = new ObservableRangeCollection<Settings>
            {
                new Settings
                {
                    Option = "Enable Push Notifications",
                    IsEnabled = Preferences.Get("enabled_push_notifications", true),
                    HelpText = "Receive App update and user notifications",
                    OnChange = (result) =>
                    {
                        Preferences.Set("enabled_push_notifications", result);
                    }
                },
                new Settings
                {
                    Option = "Enable Biometric Login",
                    IsEnabled = Preferences.Get("enabled_fingerprint_login", false),
                    HelpText = "Enable Fingerprint or Face Id Login",
                    OnChange = (result) =>
                    {
                        Preferences.Set("enabled_fingerprint_login", result);
                    }
                },
                new Settings
                {
                    Option = "Enable Background Sync",
                    IsEnabled = Preferences.Get("enabled_background_sync", false),
                    HelpText ="Sync data in the background for better performance",
                    OnChange = (result) =>
                    {
                        Preferences.Set("enabled_background_sync", result);
                    }
                }
            };
        }
    }
}
