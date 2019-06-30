using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System.Collections.ObjectModel;
using EnterpriseAddLogs.Services;

namespace EnterpriseAddLogs.ViewModels
{
    public class MenuPageViewModel: PageViewModel
    {
        private IMessageBus MessageBus { get; set; }

        public MenuPageViewModel(INavigator navigator, IMessageBus messageBus):base(navigator)
        {
            MessageBus = messageBus;
            MenuItems = new ObservableCollection<MenuItemViewModel>();

            MessageBus.Subscribe<LoginStateChangedMessage>(message =>
            {
                SetMenuItems(message.IsLoggedIn);
                SetUserProfile(message.IsLoggedIn);
            });
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

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private void SetUserProfile(bool set)
        {
            if (set)
            {
                FullName = AppService.Instance.UserIdentity.FullName;
                Email = AppService.Instance.UserIdentity.Email;
            }
            else
            {
                FullName = string.Empty;
                Email = string.Empty;
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
                ImageIcon="fas-home",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    await Navigator.NavigateToViewModelAsync<HomePageViewModel>();
                },
                IsHome = true
            });

            //MenuItems.Add(new MenuItemViewModel
            //{
            //    Title = "Log",
            //    ImageIcon = "far-list-alt",
            //    OnSelected = async () =>
            //    {
            //        MessageBus.Publish(new ShowMenuMessage(false));

            //        await Navigator.NavigateToViewModelAsync<LogIndexPageViewModel>();
            //    }
            //});

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Day Log",
                ImageIcon = "far-edit",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    await Navigator.NavigateToViewModelAsync<DayLogIndexPageViewModel>();
                }
            });

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Location",
                ImageIcon = "fas-map-marker",
                OnSelected = async() =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    await Navigator.NavigateToViewModelAsync<LocationPageViewModel>();
                }
            });

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Log Off",
                ImageIcon = "fas-sign-out-alt",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    var confirm = await Navigator.DisplayAlertAsync("Log Off", "Log Off and Exit?", "Yes", "No");

                    if (confirm)
                    {
                        await App.Authenticator.LogoutAsync();

                        MessageBus.Publish(new ExitAppMessage());
                    }
                }
            });
        }
    }
}
