namespace EnterpriseAddLogs.ViewModels
{
    using EnterpriseAddLogs.Helpers;
    using System.Threading.Tasks;

    public class PageViewModel: ViewModel
    {
        protected INavigator Navigator { get; private set; }

        public PageViewModel(INavigator navigator)
        {
            Navigator = navigator;
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }

        private string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        /// <summary>
        /// Synchronous OnNavigatedTo. Can implement either OnNavigatedTo or OnNavigatedToAsync. Or neither. No need for both.
        /// </summary>
        /// <param name="parameter">Navigation parameter</param>
        public virtual void OnNavigatedTo(object parameter = null)
        {
        }

        /// <summary>
        /// Asynchronous OnNavigatedTo. Can implement either OnNavigatedTo or OnNavigatedToAsync. Or neither. No need for both.
        /// </summary>
        /// <param name="parameter">Navigation parameter</param>
        public virtual Task OnNavigatedToAsync(object parameter = null)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Synchronous OnNavigatedBackTo. Can implement either OnNavigatedBackTo or OnNavigatedBackToAsync. Or neither. No need for both.
        /// </summary>
        /// <param name="parameter">Navigation parameter</param>
        public virtual void OnNavigatedBackTo(object parameter = null)
        {
        }

        /// <summary>
        /// Asynchronous OnNavigatedBackTo. Can implement either OnNavigatedBackTo or OnNavigatedBackToAsync. Or neither. No need for both.
        /// </summary>
        /// <param name="parameter">Navigation parameter</param>
        public virtual Task OnNavigatedBackToAsync(object parameter = null)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Synchronous OnAppearing. Can implement either OnAppearing or OnAppearingAsync. Or neither. No need for both.
        /// </summary>
        public virtual void OnAppearing()
        {
        }

        /// <summary>
        /// Asynchronous OnAppearing. Can implement either OnAppearing or OnAppearingAsync. Or neither. No need for both.
        /// </summary>
        public virtual Task OnAppearingAsync()
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Synchronous OnDisappearing. Can implement either OnDisappearing or OnDisappearingAsync. Or neither. No need for both.
        /// </summary>
        public virtual void OnDisappearing()
        {
        }

        /// <summary>
        /// Asynchronous OnDisappearing. Can implement either OnDisappearing or OnDisappearingAsync. Or neither. No need for both.
        /// </summary>
        public virtual Task OnDisappearingAsync()
        {
            return Task.FromResult(false);
        }
    }
}
