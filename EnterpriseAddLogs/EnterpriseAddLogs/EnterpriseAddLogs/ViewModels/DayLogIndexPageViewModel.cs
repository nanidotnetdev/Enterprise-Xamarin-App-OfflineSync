using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class DayLogIndexPageViewModel: PageViewModel
    {
        private ObservableRangeCollection<DayLog> _dayLogs;

        public ObservableRangeCollection<DayLog> DayLogs
        {
            get => _dayLogs;
            set => SetProperty(ref _dayLogs, value);
        }

        private DayLog _dayLogSelected;

        public DayLog DayLogSelected
        {
            get => _dayLogSelected;
            set
            {
                SetProperty(ref _dayLogSelected, value);
                OnDayLogSelected();
            }
        }

        public ICommand AddLogCommand { get; set; }

        public Command RefreshListCommand { get; set; }

        public DayLogIndexPageViewModel(INavigator navigator) : base(navigator)
        {
            _dayLogs = new ObservableRangeCollection<DayLog>();
            AddLogCommand = new Command(AddDayLogPageCommand);
            RefreshListCommand = new Command(RefreshDayLogs);
            ExecuteLoadDayLogs();
        }

        private async void AddDayLogPageCommand()
        {
            await Navigator.NavigateToViewModelAsync<DayLogCreatePageViewModel>();
        }

        private async void RefreshDayLogs()
        {
            IsBusy = true;
            await ExecuteLoadDayLogs();
            IsBusy = false;
        }

        public async Task ExecuteLoadDayLogs()
        {
            UserDialogs.Instance.ShowLoading();

            ICollection<DayLog> logs = await AppService.Instance.DayLog.GetItemsAsync(true);
            DayLogs.ReplaceRange(logs.OrderByDescending(l => l.CreatedAt));

            UserDialogs.Instance.HideLoading();
        }

        public async Task OnDayLogSelected()
        {
            await Navigator.NavigateToViewModelAsync<DayLogCreatePageViewModel>(DayLogSelected);
        }
    }
}
