using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EnterpriseAddLogs.ViewModels;
using Autofac;

namespace EnterpriseAddLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogCreatePage : ContentPage
	{
		public LogCreatePage ()
		{
			InitializeComponent ();
            
            //BindingContext = Ioc.Container.Resolve<LogCreatePageViewModel>();
        }
        
    }
}