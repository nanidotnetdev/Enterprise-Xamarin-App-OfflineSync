using Autofac;
using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Services;
using EnterpriseAddLogs.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace EnterpriseAddLogs
{
	public partial class App : Application
	{
        public static IAuthenticate Authenticator { get; private set; }

        public App ()
		{
			InitializeComponent();

            //Authenticator = Ioc.Resolve<IAuthenticate>();
            Authenticator = DependencyService.Get<IAuthenticate>();
            MainPage = Ioc.Resolve<IViewResolver>().ResolveView<MainPageViewModel>();
            MainPage.BindingContext = Ioc.Resolve<MainPageViewModel>();
            Ioc.Resolve<INavigator>().Navigation = MainPage.Navigation;

            AppCenter.Start("android=70d18fca-e15e-47dd-aac6-c07748c9c3f2;",
                  typeof(Analytics), typeof(Crashes));
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
