using EnterpriseAddLogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnterpriseAddLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();

            BindingContext = Ioc.Resolve<MenuPageViewModel>();
		}

        private void MenuItemsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView c)
                c.SelectedItem = null;
        }
    }
}