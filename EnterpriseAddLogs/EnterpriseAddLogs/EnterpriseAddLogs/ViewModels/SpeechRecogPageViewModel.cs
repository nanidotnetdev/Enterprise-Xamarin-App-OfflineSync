using System;
using System.Diagnostics;
using System.Windows.Input;
using EnterpriseAddLogs.Helpers;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class SpeechRecogPageViewModel : PageViewModel
    {
        public ICommand StartSpeechRecog { get; }

        public ICommand StopSpeechRecog { get; }

        private string _comment = string.Empty;

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        private IDisposable speechObject;

        //private ISpeechRecognizer speechRecognizer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigator"></param>
        /// <param name="speech"></param>
        public SpeechRecogPageViewModel(INavigator navigator) : base(navigator)
        {
            StartSpeechRecog = new Command(startSpeechRecog);
            StopSpeechRecog = new Command(stopSpeechRecog);

            //    .SubOnMainThread(
            //    x => this.Comment += " " + x,
            //    ex => this.Comment = ex.ToString()
            //);
        }

        private async void startSpeechRecog()
        {
            //var permission = await speechRecognizer.RequestAccess();

            //try
            //{
            //    if (permission == AccessState.Available)
            //    {
            //        speechObject = speechRecognizer.ContinuousDictation()
            //            .Subscribe(x => this.Comment = x);
            //    }
            //    else
            //    {
            //        Notifications.ErrorToast("Speech Recog Not Supported!");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        private async void stopSpeechRecog()
        {
            speechObject.Dispose();
            speechObject = null;
        }
    }
}
