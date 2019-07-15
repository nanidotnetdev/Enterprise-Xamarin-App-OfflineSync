using EnterpriseAddLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnterpriseAddLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
        }
        protected override bool OnBackButtonPressed()
        {
            var viewModel = (HomePageViewModel)BindingContext;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await viewModel.OnBackButtonPressed();
            });

            return true;
        }
    }
}