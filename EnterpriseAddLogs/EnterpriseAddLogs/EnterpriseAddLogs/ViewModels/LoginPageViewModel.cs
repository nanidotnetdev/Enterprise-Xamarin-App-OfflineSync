using EnterpriseAddLogs.Commands;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class LoginPageViewModel: PageViewModel
    {
        bool authenticated = false;

        private string _messageLabel = "Please Login!";

        public string MessageLabel {
            get => _messageLabel;
            set => SetProperty(ref _messageLabel, value);
        }

        public LoginPageViewModel(INavigator navigator, IMessageBus messageBus)
            :base(navigator)
        {
            MessageBus = messageBus;

            FingerprintLogin();
        }

        private IMessageBus MessageBus { get; set; }

        public ICommand LoginCommand => new AsyncActionCommand(LoginAsync);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task FingerprintLogin()
        {
            if (App.Authenticator != null)
            {
                authenticated = await App.Authenticator.FingerPrintLogin();
                await UserAuthenticated(authenticated);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            try
            {
                if (App.Authenticator != null)
                {
                    authenticated = await App.Authenticator.AuthenticateAsync();

                    await UserAuthenticated(authenticated);
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                {
                    MessageLabel = "Authentication cancelled by the user";
                }
            }
            catch (Exception ex)
            {
                MessageLabel = "Authentication failed";
            }
        }

        private async Task UserAuthenticated(bool authenticated)
        {
            if (authenticated)
            {
                MessageBus.Publish(new LoginStateChangedMessage(true));
                CrossLocalNotifications.Current.Show("Enterprise Add Logs", "Welcome to the App");
                await Navigator.NavigateToDetailViewModelAsync<HomePageViewModel>();
            }
        }
    }
}