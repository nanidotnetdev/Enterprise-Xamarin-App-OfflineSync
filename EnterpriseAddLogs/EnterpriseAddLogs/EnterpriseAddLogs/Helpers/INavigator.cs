namespace EnterpriseAddLogs.Helpers
{
    using EnterpriseAddLogs.ViewModels;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public interface INavigator
    {
        INavigation Navigation { get; set; }
        void Remove(PageViewModel viewModel);
        Task CloseAsync(object parameter = null);
        Task CloseModalAsync(object parameter = null);
        Task NavigateToDetailViewModelAsync<TViewModel>(object parameter = null) where TViewModel : PageViewModel;
        Task NavigateToModalViewModelAsync<TViewModel>(object parameter = null) where TViewModel : PageViewModel;
        Task NavigateToViewModelAsync<TViewModel>(object parameter = null) where TViewModel : PageViewModel;
        Task DisplayAlertAsync(string title, string message, string cancel);
        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

    }
}
