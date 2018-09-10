namespace EnterpriseAddLogs.Helpers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using EnterpriseAddLogs.ViewModels;
    using EnterpriseAddLogs.Messaging;

    public sealed class Navigator : INavigator
    {
        public INavigation Navigation { get; set; }

        private IViewResolver ViewResolver { get; set; }

        private IMessageBus MessageBus { get; set; }


        public Navigator(IViewResolver viewResolver, IMessageBus messageBus)
        {
            ViewResolver = viewResolver;
            MessageBus = messageBus;
        }

        public async Task NavigateToViewModelAsync<TViewModel>(object parameter = null)
            where TViewModel : PageViewModel
        {
            await NavigateAsync<TViewModel>(false, parameter);
        }

        public async Task NavigateToModalViewModelAsync<TViewModel>(object parameter = null)
            where TViewModel : PageViewModel
        {
            await NavigateAsync<TViewModel>(true, parameter);
        }

        public async Task NavigateToDetailViewModelAsync<TViewModel>(object parameter = null)
            where TViewModel : PageViewModel
        {
            //Logger.Track("NavigateToDetailViewModel", new Dictionary<string, string> { { "Page", typeof(TViewModel).Name } });

            var view = ViewResolver.ResolveView<TViewModel>();

            if (view == null)
            {
                //Logger.Information("Skipping navigation to view model {ViewModelName}", typeof(TViewModel).Name);
                return;
            }

            var viewModel = Ioc.Resolve<TViewModel>();

            view.BindingContext = viewModel;

            var message = new ShowDetailPageMessage(view);
            MessageBus.Publish(message);

            viewModel.OnNavigatedTo(parameter);
            await viewModel.OnNavigatedToAsync(parameter);
        }


        public void Remove(PageViewModel viewModel)
        {
            var page = Navigation.NavigationStack.Single(_ => _.BindingContext == viewModel);
            Navigation.RemovePage(page);
        }

        public async Task CloseAsync(object parameter = null)
        {
            await Navigation.PopAsync(true);

            var newCurrentPageViewModel = (PageViewModel)Navigation.NavigationStack.Last().BindingContext;
            newCurrentPageViewModel.OnNavigatedBackTo(parameter);
            await newCurrentPageViewModel.OnNavigatedBackToAsync(parameter);
        }

        public async Task CloseModalAsync(object parameter = null)
        {
            await Navigation.PopModalAsync(true);

            PageViewModel newCurrentPageViewModel = null;
            if (Navigation.ModalStack.Count > 0)
            {
                newCurrentPageViewModel = (PageViewModel)Navigation.ModalStack.Last().BindingContext;
            }
            else
            {
                newCurrentPageViewModel = (PageViewModel)Navigation.NavigationStack.Last().BindingContext;
            }

            newCurrentPageViewModel.OnNavigatedBackTo(parameter);
            await newCurrentPageViewModel.OnNavigatedBackToAsync(parameter);
        }

        private async Task NavigateAsync<TViewModel>(bool isModal, object parameter)
            where TViewModel : PageViewModel
        {
            var view = ViewResolver.ResolveView<TViewModel>();

            if (view == null)
            {
                //Logger.Information("Skipping navigation to view model {ViewModelName}", typeof(TViewModel).Name);
                return;
            }

            var viewModel = Ioc.Resolve<TViewModel>();

            view.BindingContext = viewModel;

            view.Appearing += async delegate
            {
                viewModel.OnAppearing();
                await viewModel.OnAppearingAsync();
            };
            view.Disappearing += async delegate
            {
                viewModel.OnDisappearing();
                await viewModel.OnDisappearingAsync();
            };

            if (isModal)
            {
                await Navigation.PushModalAsync(view);
            }
            else
            {
                if (Navigation.NavigationStack.Last().GetType().Name == view.GetType().Name)
                {
                    // Already on the requested page.
                    return;
                }

                await Navigation.PushAsync(view);
            }

            viewModel.OnNavigatedTo(parameter);
            await viewModel.OnNavigatedToAsync(parameter);
        }

        public async Task DisplayAlertAsync(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
