using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.ViewModels
{
    public class HomePageViewModel : PageViewModel
    {
        private IMessageBus MessageBus { get; set; }

        public HomePageViewModel(INavigator navigator, IMessageBus messageBus) : base(navigator)
        {
            MessageBus = messageBus;
        }

        public async Task OnBackButtonPressed()
        {
            var shouldExitApp = await Navigator.DisplayAlertAsync("Exit?", "Leave the App?", "yes", "No");

            if (shouldExitApp)
            {
                MessageBus.Publish(new ExitAppMessage());
            }
        }
    }
}
