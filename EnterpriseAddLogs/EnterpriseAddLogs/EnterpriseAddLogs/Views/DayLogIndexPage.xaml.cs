using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnterpriseAddLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayLogIndexPage  : ContentPage
    {
        public DayLogIndexPage()
        {
            InitializeComponent();
        }

        private void DayLogListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(sender is ListView c)
                c.SelectedItem = null;
        }
    }
}