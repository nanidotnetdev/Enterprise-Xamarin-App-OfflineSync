namespace EnterpriseAddLogs.Views
{
    using Autofac;
    using EnterpriseAddLogs.Models;
    using EnterpriseAddLogs.ViewModels;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogIndexPage : ContentPage
	{
		public LogIndexPage()
		{
			InitializeComponent ();

            //BindingContext = Ioc.Container.Resolve<LogIndexPageViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        //private async void Log_Selected(object sender, ItemTappedEventArgs e)
        //{
        //    var selectedLog = e.Item as LogEntity;

        //    if (selectedLog == null)
        //        return;

        //    var viewModel = (LogIndexPageViewModel)BindingContext;
        //    viewModel.LogEntity = selectedLog;
        //    LogListView.SelectedItem = null;
        //    await viewModel.LogPageAsync();

        //}

        //private async void btnAddLogClicked(object sender, System.EventArgs e)
        //{
        //    await Navigation
        //}
    }
}