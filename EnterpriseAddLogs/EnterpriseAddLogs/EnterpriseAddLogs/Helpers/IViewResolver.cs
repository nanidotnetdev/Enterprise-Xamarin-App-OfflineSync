namespace EnterpriseAddLogs.Helpers
{
    using EnterpriseAddLogs.ViewModels;
    using Xamarin.Forms;

    public interface IViewResolver
    {
        Page ResolveView<TViewModel>() where TViewModel : PageViewModel;
    }
}
