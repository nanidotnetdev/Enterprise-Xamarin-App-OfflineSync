namespace EnterpriseAddLogs.ViewModels
{
    using EnterpriseAddLogs.Helpers;
    using EnterpriseAddLogs.Models;
    using EnterpriseAddLogs.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class LogViewModel: PageViewModel
    {
        private ObservableCollection<LogEntity> _logs { get; set; }

        public ObservableCollection<LogEntity> Logs
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

        private LogEntity _logEntity;

        public LogEntity LogEntity
        {
            get { return _logEntity; }
            set
            {
                _logEntity = value;
                NotifyPropertyChanged();
            }
        }

        private ILogService _logService;

        public LogViewModel(ILogService logService, INavigator navigator)
            :base(navigator)
        {
            _logService = logService;

            Title = "Logs";

            _logs = new ObservableCollection<LogEntity>();

            ExecuteLoadAllLogs();
        }

        private async void ExecuteLoadAllLogs()
        {
            ICollection<LogEntity> logs = await _logService.GetAllLogsAsync();

            Logs.Clear();

            foreach (var log in logs)
            {
                Logs.Add(log);
            }
        }

        public async Task LogPageAsync()
        {
            await Navigator.NavigateToViewModelAsync<LogCreateViewModel>(LogEntity);
        }


    }
}
