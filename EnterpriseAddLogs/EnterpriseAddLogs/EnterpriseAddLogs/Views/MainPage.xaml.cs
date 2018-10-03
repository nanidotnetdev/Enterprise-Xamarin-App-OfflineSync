using Autofac;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using Xamarin.Forms;

namespace EnterpriseAddLogs.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            var messageBus = Ioc.Container.Resolve<IMessageBus>();
            messageBus.Subscribe<ShowDetailPageMessage>(message =>
            {
                var navigationPage = new NavigationPage(message.Page);
                Ioc.Resolve<INavigator>().Navigation = navigationPage.Navigation;
                Detail = navigationPage;
                IsPresented = false;
            });

            messageBus.Subscribe<ShowMenuMessage>(message =>
            {
                IsPresented = message.ShowMenu;
            });
        }
        //}
    }
}
