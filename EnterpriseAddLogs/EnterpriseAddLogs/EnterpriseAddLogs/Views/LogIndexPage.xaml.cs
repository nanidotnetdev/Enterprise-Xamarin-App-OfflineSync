namespace EnterpriseAddLogs.Views
{
    using Autofac;
    using EnterpriseAddLogs.ViewModels;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogIndexPage : ContentPage
	{
		public LogIndexPage()
		{
			InitializeComponent ();

            BindingContext = Ioc.Container.Resolve<LogViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private void UserListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void btnAddLogClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new LogCreatePage());
        }
    }
}