namespace EnterpriseAddLogs.ViewModels
{
    using EnterpriseAddLogs.Helpers;
    using EnterpriseAddLogs.Models;
    using EnterpriseAddLogs.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LogIndexPageViewModel : PageViewModel
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

        public ICommand AddLogCommand { get; set; }


        public LogIndexPageViewModel(ILogService logService, INavigator navigator)
            :base(navigator)
        {
            _logService = logService;

            Title = "Logs";

            _logs = new ObservableCollection<LogEntity>();

            AddLogCommand = new Command(AddLogPageCommand);

            ExecuteLoadAllLogs();
        }

        private async void AddLogPageCommand()
        {
            await Navigator.NavigateToDetailViewModelAsync<LogCreatePageViewModel>();
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
            await Navigator.NavigateToViewModelAsync<LogCreatePageViewModel>(LogEntity);
        }

    }
}
