using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using System.Collections.ObjectModel;
using EnterpriseAddLogs.Services;

namespace EnterpriseAddLogs.ViewModels
{
    public class MenuPageViewModel: PageViewModel
    {
        private IMessageBus MessageBus { get; set; }

        private readonly ISecurityService _securityService;

        public MenuPageViewModel(INavigator navigator, IMessageBus messageBus, ISecurityService securityService):base(navigator)
        {
            MessageBus = messageBus;
            MenuItems = new ObservableCollection<MenuItemViewModel>();
            _securityService = securityService;

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
                FullName = _securityService.CurrentUser.FullName;
                Email = _securityService.CurrentUser.Email;
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

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Azure Mobile Apps",
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
                Title = "Speech Recog",
                ImageIcon = "fas-map-marker",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    await Navigator.NavigateToViewModelAsync<SpeechRecogPageViewModel>();
                }
            });

            MenuItems.Add(new MenuItemViewModel
            {
                Title = "Log Off",
                ImageIcon = "fas-sign-out-alt",
                OnSelected = async () =>
                {
                    MessageBus.Publish(new ShowMenuMessage(false));

                    var confirm = await Navigator.DisplayAlertAsync("Log Off", "Log Off?", "Yes", "No");

                    if (confirm)
                    {
                        await App.Authenticator.LogoutAsync();
                        MessageBus.Publish(new LoginStateChangedMessage(false));
                        await Navigator.NavigateToDetailViewModelAsync<LoginPageViewModel>();
                    }
                }
            });
        }
    }
}