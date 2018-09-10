using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.ViewModels
{
    public sealed class MainPageViewModel: PageViewModel
    {
        public MainPageViewModel(INavigator navigator, IMessageBus messageBus): base(navigator)
        {
            messageBus.Subscribe<LoginStateChangedMessage>(async message =>
            {
                if (message.IsLoggedIn)
                {

                }
                else
                {
                    await Navigator.NavigateToDetailViewModelAsync<LoginPageViewModel>();
                }
            });
        }
    }
}
