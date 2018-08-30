using EnterpriseAddLogs.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EnterpriseAddLogs
{
	public partial class MainPage : MasterDetailPage
    {
		public MainPage()
		{
			InitializeComponent();

            var navigationPage = new NavigationPage();

            Ioc.Resolve<INavigator>().Navigation = navigationPage.Navigation;
            Detail = navigationPage;


            //var messageBus = Ioc.Container.Resolve<IMessageBus>();
            //messageBus.Subscribe<ShowDetailPageMessage>(message =>
            //{
            //    var navigationPage = new NavigationPage(message.Page);
            //    Ioc.Resolve<INavigator>().Navigation = navigationPage.Navigation;
            //    Detail = navigationPage;
            //    IsPresented = false;
            //});

        }
	}
}
