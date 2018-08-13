namespace EnterpriseAddLogs.Views
{
    using Autofac;
    using Autofac.Core;
    using EnterpriseAddLogs.ViewModels;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogIndexPage : ContentPage
	{
        private LogViewModel logViewModel;

        private IContainer _container;

		public LogIndexPage()
		{
			InitializeComponent ();

            //_container = container;

            //BindingContext = logViewModel = _container.Resolve<LogViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}