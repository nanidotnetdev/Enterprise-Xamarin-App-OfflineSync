using EnterpriseAddLogs.Commands;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using EnterpriseAddLogs.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseAddLogs.ViewModels
{
    public class LoginPageViewModel: PageViewModel
    {
        bool authenticated = false;

        private string _messageLabel;

        public string MessageLabel {
            get {
                return _messageLabel;
            }
            set {
                _messageLabel = value;
            }
        }

        public LoginPageViewModel(INavigator navigator, IMessageBus messageBus):base(navigator)
        {
            MessageBus = messageBus;

            FingerprintLogin();
        }

        private async void FingerprintLogin()
        {
            var authenticated = await App.Authenticator.FingerPrintLogin();

            if (authenticated)
            {
                MessageBus.Publish(new LoginStateChangedMessage(true));
                await Navigator.NavigateToDetailViewModelAsync<HomePageViewModel>();
            }
        }

        private IMessageBus MessageBus { get; set; }

        public ICommand LoginCommand => new AsyncActionCommand(LoginAsync);

        private async Task LoginAsync()
        {
            try
            {
                if (App.Authenticator != null)
                {
                    authenticated = await App.Authenticator.AuthenticateAsync();
                }

                if (authenticated)
                {
                    MessageBus.Publish(new LoginStateChangedMessage(true));
                    await Navigator.NavigateToDetailViewModelAsync<HomePageViewModel>();
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                {
                    MessageLabel = "Authentication cancelled by the user";
                }
            }
            catch (Exception)
            {
                MessageLabel = "Authentication failed";
            }

        }
    }
}
