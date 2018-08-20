using EnterpriseAddLogs.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace EnterpriseAddLogs
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new MainPage();
            MainPage.BindingContext = Ioc.Resolve<MainPage>();
            Ioc.Resolve<INavigator>().Navigation = MainPage.Navigation;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
