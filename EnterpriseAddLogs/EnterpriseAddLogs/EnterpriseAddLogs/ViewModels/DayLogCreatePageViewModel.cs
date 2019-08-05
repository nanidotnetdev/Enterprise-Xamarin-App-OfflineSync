using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;
using Microsoft.WindowsAzure.Storage.Blob;

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

        public FileSource LastSelectedImage { get; set; }

        public ICommand SaveCommand { get; set; }

        //public ICommand PickFiles { get; set; }

        //public ICommand TakePhoto { get; set; }

        public ICommand ImageTapped { get; set; }

        public ICommand FileUploadOptions { get; set; }

        private ObservableCollection<FileSource> fileList { get; set; }

        public ObservableCollection<FileSource> FileList
        {
            get => fileList;
            set => fileList = value;
        }

        private IDayLogService _dayLogService;

        public DayLogCreatePageViewModel(INavigator navigator, IDayLogService dayLogService):base(navigator)
        {
            _dayLogService = dayLogService;
            SaveCommand = new Command(SaveDayLogAsync);
            //PickFiles = new Command(PickPhotoCommand);
            //TakePhoto = new Command(TakePhotoCommand);
            FileList = new ObservableCollection<FileSource>();
            ImageTapped = new Command(ShowImageOptions);
            FileUploadOptions = new Command(ShowFileOptions);

            if (DayLogEntity == null)
                DayLogEntity = new DayLog
                {
                    DayLogId = Guid.NewGuid(),
                    IsNew = true
                };
        }

        private void ShowImageOptions()
        {
            UserDialogs.Instance
                .ActionSheet(new ActionSheetConfig()
                    .SetCancel()
                    .Add("View", OpenFullScreenImageView)
                    .Add("Delete", async () => await DeleteImage()));
        }

        private async Task DeleteImage()
        {
            await StorageService.DeleteFile(LastSelectedImage.Text);
            fileList.Remove(LastSelectedImage);
        }

        private void OpenFullScreenImageView()
        {
            FullScreenImageView.Show(FileList, fileList.IndexOf(LastSelectedImage));
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

            DayLogEntity.Comment = Comment;
            DayLogEntity.DateLogged = DateLogged;
            DayLogEntity.DayTimeId = DayLogTimeSelected?.DayTimeId;

            //save daylog
            await _dayLogService.UpsertAsync(DayLogEntity);

            Notifications.BusyIndicator(false);
            Notifications.SuccessToast("Saved");
        }

        public override async Task OnNavigatedToAsync(object parameter = null)
        {
            if(parameter != null)
            {
                Notifications.BusyIndicator();

                if (parameter is DayLog selLog)
                {
                    //DayLogEntity = _dayLogService.GetById(selLog.id).Result;
                    DayLogEntity = selLog;

                    Comment = DayLogEntity.Comment;
                    DateLogged = DayLogEntity.DateLogged;
                    DayLogTimeSelected = DayLogTimes.FirstOrDefault(d => d.DayTimeId == DayLogEntity.DayTimeId);

                    var fl = await StorageService.GetBlobs<CloudBlockBlob>(DayLogEntity.DayLogId);

                    foreach (var fileSource in fl)
                    {
                        fileList.Add(fileSource);
                    }
                }

                Notifications.BusyIndicator(false);
            }
        }

        private void ShowFileOptions()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                .SetCancel()
                .Add("Take Photo", TakePhoto)
                .Add("Upload Photo", PickPhoto)
                .Add("Take Video", TakeVideo)
                .Add("Pick Video", PickVideo));
        }

        private async void TakePhoto()
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

            await SaveFile(file);
        }

        private async void TakeVideo()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable)
            {
                Notifications.ErrorToast("No Camera Available!");
                return;
            }

            if (!CrossMedia.Current.IsTakeVideoSupported)
            {
                Notifications.ErrorToast("Can't take Videos!");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                Directory = "eod",
                DefaultCamera = CameraDevice.Rear,
                AllowCropping = true
            });

            await SaveFile(file);
        }

        private async void PickPhoto()
        {
            await CrossMedia.Current.Initialize();

            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

            await SaveFile(file);
        }

        private async void PickVideo()
        {
            await CrossMedia.Current.Initialize();

            MediaFile file = await CrossMedia.Current.PickVideoAsync();

            await SaveFile(file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task SaveFile(MediaFile file)
        {
            if (file == null)
                return;

            Notifications.BusyIndicator(title:"uploading..");

            string fileName = DateTime.Now.Ticks.ToString();

            var saved = await StorageService.UploadFile(new FileSource { FilePath = file.Path, Text = fileName }, DayLogEntity.DayLogId);

            fileList.Add(saved);

            Notifications.BusyIndicator(false);
        }
    }
}
