using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAddLogs.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnterpriseAddLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            var s = sender as Switch;
            var item = s?.Parent.BindingContext as Settings;
            item?.OnChange(s.IsToggled);
        }
    }
}