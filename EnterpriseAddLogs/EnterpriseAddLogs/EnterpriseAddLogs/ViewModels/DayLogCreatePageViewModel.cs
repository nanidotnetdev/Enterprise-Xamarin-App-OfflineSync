using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.SpeechRecognition;
using System;
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
        private string _comment;

        public string Comment
        {
            get => _comment;
            set
            {
                SetProperty(ref _comment, value);
            }
        }

        private DateTime _dateLogged { get; set; } = DateTime.Now;

        public DateTime DateLogged
        {
            get => _dateLogged;
            set
            {
                _dateLogged = value;
                NotifyPropertyChanged();
            }
        }

        private DayLogTime _dayTimeSelected;

    public DayLogTime DayLogTimeSelected
        {
            get => _dayTimeSelected;
            set => SetProperty(ref _dayTimeSelected, value);
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
            get => _dayLogTimes;
            set
            {
                _dayLogTimes = value;
                NotifyPropertyChanged();
            }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
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

        public DayLogCreatePageViewModel(INavigator navigator):base(navigator)
        {
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
            get => _dayLogEntity;
            set => _dayLogEntity = value;
        }

        public async void SaveDayLogAsync()
        {
            //required field validation
            if (string.IsNullOrEmpty(Comment))
            {
                Notifications.ErrorToast("Comment Required.");
                return;
            }

            Notifications.BusyIndicator(title:"Saving..");
            //UserDialogs.Instance.ShowLoading("Saving..");

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

            await AppService.Instance.DayLog.UpsertAsync(DayLogEntity);

            await Navigator.CloseAsync();

            //await Navigator.CloseAsync();

            Notifications.BusyIndicator(false);
            Notifications.SuccessToast("Saved");
        }

        public override Task OnNavigatedToAsync(object parameter = null)
        {
            if(parameter != null)
            {
                UserDialogs.Instance.ShowLoading();

                if(parameter is DayLog selLog)
                {
                    //DayLogEntity = _dayLogService.GetById(selLog.id).Result;
                    DayLogEntity = selLog;

                    Comment = DayLogEntity.Comment;
                    DateLogged = DayLogEntity.DateLogged;
                    DayLogTimeSelected = DayLogTimes.FirstOrDefault(d => d.DayTimeId == DayLogEntity.DayTimeId);

                    var fl = await StorageService.GetBlobs<CloudBlockBlob>(DayLogEntity.DayLogId, "offlinesyncapp");

                    foreach (var fileSource in fl)
                    {
                        fileList.Add(fileSource);
                    }
                }
                UserDialogs.Instance.HideLoading();

            }
        }

        private async void TakePhotoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable)
            {
                Notifications.ErrorToast("No Camera Available!");
                return;
            }

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                Notifications.ErrorToast("Can't take Photos!");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "eod",
                DefaultCamera = CameraDevice.Rear
            });

            SaveFile(file);
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

            await SaveFile(file);
        }

        private async Task SaveFile(MediaFile file)
        {
            if (file == null)
                return;

            string fileName = DateTime.Now.Ticks.ToString();

            var fil = new FileSource {FilePath = file.Path, Image = ImageSource.FromFile(file.Path), Text = fileName};

            fileList.Add(fil);

            await StorageService.UploadFile(fil, DayLogEntity.DayLogId);
        }
    }
}
