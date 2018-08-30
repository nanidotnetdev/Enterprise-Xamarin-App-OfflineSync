using EnterpriseAddLogs.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.ViewModels
{
    public sealed class MainPageViewModel: PageViewModel
    {
        public MainPageViewModel(INavigator navigator): base(navigator)
        {
            Navigator.NavigateToDetailViewModelAsync<HomePageViewModel>();
        }
    }
}
