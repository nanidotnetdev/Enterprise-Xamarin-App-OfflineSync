using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class DayLogCreatePageViewModel: PageViewModel
    {
        private IDayLogService _dayLogService;
        private string _comment;

        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _dateLogged;

        public DateTime DateLogged
        {
            get
            {
                return _dateLogged;
            }
            set
            {
                _dateLogged = value;
                NotifyPropertyChanged();
            }
        }

        private DayLogTime _dayTimeSelected { set; get; }

        public DayLogTime DayLogTimeSelected
        {
            get
            {
                return _dayTimeSelected;
            }
            set
            {
                _dayTimeSelected = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<DayLogTime> _dayLogTimes { get; set; } = new ObservableCollection<DayLogTime>
        {
            new DayLogTime
            {
                Text = "Morning",
                DayTimeId = Guid.Parse("A2C53233-9766-4702-A259-8C6E3A5B515F")
            },
            new DayLogTime
            {
                Text ="Afternoon",
                DayTimeId = Guid.Parse("EFA16C3E-660A-44A4-8A9A-90B4C4145DD4")
            },
            new DayLogTime
            {
                Text = "Evening",
                DayTimeId = Guid.Parse("30945293-85F8-44C8-BE35-D13C97F68ED3")
            }
        };

        public ObservableCollection<DayLogTime> DayLogTimes
        {
            get
            {
                return new ObservableCollection<DayLogTime>(_dayLogTimes);
            }
            set
            {
                _dayLogTimes = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }

        public DayLogCreatePageViewModel(IDayLogService dayLogService,INavigator navigator):base(navigator)
        {
            _dayLogService = dayLogService;
            SaveCommand = new Command(SaveDayLogAsync);

            //DayLogTimeSelected = _dayLogTimes[1];
        }

        public async void SaveDayLogAsync()
        {
            IsBusy = true;

            var dayLog = new DayLog
            {
                Comment = _comment,
                DateLogged = _dateLogged
            };

            var it = _dayTimeSelected;

            await _dayLogService.SaveDayLog(dayLog);

            await Navigator.CloseAsync();

            await Navigator.NavigateToViewModelAsync<DayLogIndexPageViewModel>();

        }
    }
    public class DayLogTime
    {
        public Guid DayTimeId { get; set; }
        public string Text { get; set; }
    }
}
