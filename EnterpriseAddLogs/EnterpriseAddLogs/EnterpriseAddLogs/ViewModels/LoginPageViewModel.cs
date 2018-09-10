using EnterpriseAddLogs.Commands;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseAddLogs.ViewModels
{
    public class LoginPageViewModel: PageViewModel
    {
        public LoginPageViewModel(INavigator navigator, IMessageBus messageBus):base(navigator)
        {
            MessageBus = messageBus;
        }

        private IMessageBus MessageBus { get; set; }

        public ICommand LoginCommand => new AsyncActionCommand(LoginAsync);

        private async Task LoginAsync()
        {
            await Navigator.NavigateToDetailViewModelAsync<HomePageViewModel>();

        }
    }
}
