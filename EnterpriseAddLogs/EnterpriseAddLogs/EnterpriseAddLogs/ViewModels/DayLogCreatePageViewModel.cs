using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;

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

        private DateTime _dateLogged { get; set; } = DateTime.Now;

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
                return _dayLogTimes;
            }
            set
            {
                _dayLogTimes = value;
                NotifyPropertyChanged();
            }
        }

        private int _selectedIndex { get; set; }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }

        public ICommand SpeechRecog { get; private set; }

        public ICommand PickFiles { get; set; }

        public ICommand TakePhoto { get; set; }

        private ObservableCollection<FileSource> fileList { get; set; }

        public ObservableCollection<FileSource> FileList
        {
            get => fileList;
            set => fileList = value;
        }

        public DayLogCreatePageViewModel(IDayLogService dayLogService,INavigator navigator):base(navigator)
        {
            _dayLogService = dayLogService;
            SaveCommand = new Command(SaveDayLogAsync);
            SpeechRecog = new Command(speechRecog);
            PickFiles = new Command(PickFilesCommand);
            TakePhoto = new Command(TakePhotoCommand);
            FileList = new ObservableCollection<FileSource>();
        }

        private async void speechRecog()
        {
            var per = CrossSpeechRecognition.Current.RequestPermission();

            var supported = CrossSpeechRecognition.Current.IsSupported;

            if (supported)
            {
                var listener = CrossSpeechRecognition
                                    .Current
                                    .ContinuousDictation()
                                    .Subscribe(phrase => {
                                        Comment += phrase;
                                        // will keep returning phrases as pause is observed
                                    });

            }
            else
            {
                var config = new ToastConfig("Speech Recog Not Supported!").SetBackgroundColor(Color.Red);
                UserDialogs.Instance.Toast(config);
            }

            var tee = string.Empty;
        }

        private DayLog _dayLogEntity { get; set; }

        public DayLog DayLogEntity
        {
            get
            {
                return _dayLogEntity;
            }
            set
            {
                _dayLogEntity = value;
            }
        }

        public async void SaveDayLogAsync()
        {
            //required field validation
            if (string.IsNullOrEmpty(Comment))
            {
                UserDialogs.Instance.Toast(NotificationConfig.ErrorToast("Comment Required"));
                return;
            }

            UserDialogs.Instance.ShowLoading("Saving..");

            if(DayLogEntity == null)
            {
                DayLogEntity = new DayLog
                {
                    Comment = Comment,
                    DateLogged = DateLogged,
                    DayTimeId = DayLogTimeSelected?.DayTimeId
                };
            }
            else
            {
                DayLogEntity.Comment = Comment;
                DayLogEntity.DateLogged = DateLogged;
                DayLogEntity.DayTimeId = DayLogTimeSelected?.DayTimeId;
            }
            
            await _dayLogService.SaveDayLog(DayLogEntity);

            await Navigator.CloseAsync();

            UserDialogs.Instance.HideLoading();
        }

        public override Task OnNavigatedToAsync(object parameter = null)
        {
            if(parameter != null)
            {
                UserDialogs.Instance.ShowLoading();

                var selLog = parameter as DayLog;

                if(selLog != null)
                {
                    //DayLogEntity = _dayLogService.GetById(selLog.id).Result;
                    DayLogEntity = selLog;

                    Comment = DayLogEntity.Comment;
                    DateLogged = DayLogEntity.DateLogged;
                    DayLogTimeSelected = DayLogTimes.FirstOrDefault(d => d.DayTimeId == DayLogEntity.DayTimeId);
                }
                
                UserDialogs.Instance.HideLoading();
            }

            return base.OnNavigatedToAsync(parameter);
        }

        private async void TakePhotoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable)
            {
                UserDialogs.Instance.Toast(NotificationConfig.ErrorToast("No Camera Available!"));
                return;
            }

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                UserDialogs.Instance.Toast(NotificationConfig.ErrorToast("Can't take Photos!"));
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "eod",
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;

            string fileName = DateTime.Now.Ticks.ToString();

            fileList.Add(new FileSource { FilePath = file.Path, Image = ImageSource.FromFile(file.Path), Text = fileName });
        }

        private async void PickFilesCommand()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                var config = new ToastConfig("No Camera Available!").SetBackgroundColor(Color.Red);
                UserDialogs.Instance.Toast(config);
                return;
            }

            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

            if (file == null)
                return;

            string fileName = DateTime.Now.Ticks.ToString();

            fileList.Add(new FileSource { FilePath = file.Path, Image = ImageSource.FromFile(file.Path), Text = fileName });
        }
    }
}
