using Autofac;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.ViewModels;
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

            MainPage = Ioc.Resolve<IViewResolver>().ResolveView<MainPageViewModel>();
            MainPage.BindingContext = Ioc.Resolve<MainPageViewModel>();
            Ioc.Resolve<INavigator>().Navigation = MainPage.Navigation;
        }

		protected override async void OnStart ()
		{
            // Handle when your app starts
            await Ioc.Container.Resolve<INavigator>().NavigateToDetailViewModelAsync<LoginPageViewModel>();

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
