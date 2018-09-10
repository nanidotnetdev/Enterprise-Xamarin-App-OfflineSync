using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EnterpriseAddLogs.ViewModels
{
    public class MenuPageViewModel: PageViewModel
    {
        private IMessageBus MessageBus { get; set; }

        public MenuPageViewModel(INavigator navigator, IMessageBus messageBus):base(navigator)
        {
            MessageBus = messageBus;
            MenuItems = new ObservableCollection<MenuItemViewModel>();

            SetMenuItems(true);
        }

        private ObservableCollection<MenuItemViewModel> _menuItems;
        public ObservableCollection<MenuItemViewModel> MenuItems
        {
            get
            {
                return _menuItems;
            }
            private set
            {
                _menuItems = value;
                NotifyPropertyChanged();
            }
        }

        private MenuItemViewModel selectedMenuItem;
        public MenuItemViewModel SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set
            {
                selectedMenuItem = value;
                NotifyPropertyChanged();

                if (value != null)
                {
                    value.OnSelected();
                    selectedMenuItem = null;
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            private set
            {
                _userName = value;
                NotifyPropertyChanged();
            }
        }

        private void SetMenuItems(bool set)
        {
            MenuItems.Clear();

            if (!set)
            {
                return;
            }

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Home",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    await Navigator.NavigateToViewModelAsync<HomePageViewModel>();
                },
                IsHome = true
            });

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Log",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    await Navigator.NavigateToViewModelAsync<LogIndexPageViewModel>();
                }
            });


        }

    }
}
