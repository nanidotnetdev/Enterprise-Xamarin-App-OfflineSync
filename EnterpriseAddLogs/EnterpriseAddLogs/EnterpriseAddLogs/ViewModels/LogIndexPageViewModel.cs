namespace EnterpriseAddLogs.ViewModels
{
    using EnterpriseAddLogs.Helpers;
    using EnterpriseAddLogs.Models;
    using EnterpriseAddLogs.Services;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LogIndexPageViewModel : PageViewModel
    {
        private ObservableRangeCollection<Log> _logs { get; set; }

        public ObservableRangeCollection<Log> Logs
        {
            get
            {
                return _logs;
            }
            set
            {
                _logs = value;
                NotifyPropertyChanged();
            }
        }

        private Log _selectedLog;

        public Log SelectedLog
        {
            get { return _selectedLog; }
            set
            {
                _selectedLog = value;
                NotifyPropertyChanged();
            }
        }

        private ILogService _logService;

        public ICommand AddLogCommand { get; set; }

        public LogIndexPageViewModel(ILogService logService, INavigator navigator)
            :base(navigator)
        {
            IsBusy = true;

            _logService = logService;

            Title = "Logs";

            _logs = new ObservableRangeCollection<Log>();

            AddLogCommand = new Command(AddLogPageCommand);

            ExecuteLoadAllLogs();
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;

                    await ExecuteLoadAllLogs();

                    IsBusy = false;
                });
            }
        }

        private async void AddLogPageCommand()
        {
            await Navigator.NavigateToViewModelAsync<LogCreatePageViewModel>();
        }

        private async Task ExecuteLoadAllLogs()
        {
            ICollection<Log> logs = await _logService.GetAllLogsAsync();

            Logs.AddRange(logs, NotifyCollectionChangedAction.Replace);

            IsBusy = false;
        }

        public async Task LogSelected()
        {
            var selLog = SelectedLog;

            await Navigator.NavigateToViewModelAsync<LogCreatePageViewModel>(SelectedLog.LogNumber);
        }

    }
}
