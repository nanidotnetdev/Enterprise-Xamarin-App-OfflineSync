using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class DayLogIndexPageViewModel: PageViewModel
    {
        private ObservableCollection<DayLog> _dayLogs { get; set; }

        public ObservableCollection<DayLog> DayLogs
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

        public ICommand AddLogCommand { get; set; }

        private IDayLogService _dayLogService;

        public DayLogIndexPageViewModel(IDayLogService dayLogService,INavigator navigator) : base(navigator)
        {
            _dayLogs = new ObservableCollection<DayLog>();
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
            ICollection<DayLog> logs = await _dayLogService.GetDayLogs();

            foreach (var log in logs)
            {
                DayLogs.Add(log);
            }
        }
    }
}
