using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class MenuItemViewModel: ViewModel
    {
        public string Title { get; set; }
        public Func<Task> OnSelected { get; set; }

        public ImageSource ImageIcon { get; set; }

        public bool IsHome { get; set; }
    }
}