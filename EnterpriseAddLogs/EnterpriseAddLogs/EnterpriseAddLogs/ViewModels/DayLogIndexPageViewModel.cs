using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class DayLogIndexPageViewModel: PageViewModel
    {
        private ObservableRangeCollection<DayLog> _dayLogs { get; set; }

        public ObservableRangeCollection<DayLog> DayLogs
        {
            get
            {
                return _dayLogs;
            }
            set
            {
                _dayLogs = value;
                NotifyPropertyChanged();
            }
        }

        private DayLog _dayLogSelected { get; set; }

        public DayLog DayLogSelected
        {
            get
            {
                return _dayLogSelected;
            }
            set
            {
                _dayLogSelected = value;
                OnDayLogSelected();
            }
        }

        public ICommand AddLogCommand { get; set; }

        private IDayLogService _dayLogService;

        public DayLogIndexPageViewModel(IDayLogService dayLogService,INavigator navigator) : base(navigator)
        {
            _dayLogs = new ObservableRangeCollection<DayLog>();
            AddLogCommand = new Command(AddDayLogPageCommand);

            _dayLogService = dayLogService;

            ExecuteLoadDayLogs();
        }

        private async void AddDayLogPageCommand()
        {
            await Navigator.NavigateToViewModelAsync<DayLogCreatePageViewModel>();
        }

        public async Task ExecuteLoadDayLogs()
        {
            UserDialogs.Instance.ShowLoading();

            ICollection<DayLog> logs = await _dayLogService.GetDayLogs();
            DayLogs.ReplaceRange(logs);

            UserDialogs.Instance.HideLoading();
        }

        public async Task OnDayLogSelected()
        {
            await Navigator.NavigateToViewModelAsync<DayLogCreatePageViewModel>(DayLogSelected);
        }
    }
}
