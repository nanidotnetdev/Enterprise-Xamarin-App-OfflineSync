using EnterpriseAddLogs.Commands;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
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
                    await Navigator.NavigateToViewModelAsync<DayLogIndexPageViewModel>();
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
