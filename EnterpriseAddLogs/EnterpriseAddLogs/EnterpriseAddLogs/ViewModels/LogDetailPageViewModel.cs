using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using EnterpriseAddLogs.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnterpriseAddLogs.ViewModels
{
    public class LogDetailPageViewModel: PageViewModel
    {
        private ObservableCollection<SystemEntity> _systemEntities { get; set; }

        public ObservableCollection<SystemEntity> SystemEntities
        {
            get
            {
                return _systemEntities;
            }
            set
            {
                _systemEntities = value;
                NotifyPropertyChanged();
            }
        }

        private SystemEntity _selectedSystem { get; set; }

        public SystemEntity SelectedSystem
        {
            get
            {
                return _selectedSystem;
            }
            set
            {
                _selectedSystem = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ComponentEntity> _componentEntities { get; set; }

        public ObservableCollection<ComponentEntity> ComponentEntities
        {
            get
            {
                return _componentEntities;
            }
            set
            {
                _componentEntities = value;
                NotifyPropertyChanged();
            }
        }

        private ComponentEntity _selectedComponent { get; set; }

        public ComponentEntity SelectedComponent
        {
            get
            {
                return _selectedComponent;
            }
            set
            {
                _selectedComponent = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<DetailTypeEntity> _detailTypeEntities { get; set; }

        public ObservableCollection<DetailTypeEntity> DetailTypeEntities
        {
            get
            {
                return _detailTypeEntities;
            }
            set
            {
                _detailTypeEntities = value;
                NotifyPropertyChanged();
            }
        }

        private DetailTypeEntity _selectedDetailType { get; set; }

        public DetailTypeEntity SelectedDetailType
        {
            get
            {
                return _selectedDetailType;
            }
            set
            {
                _selectedDetailType = value;
                NotifyPropertyChanged();
            }
        }

        private string _detailComment;

        public string DetailComment
        {
            get
            {
                return _detailComment;
            }
            set
            {
                _detailComment = value;
                NotifyPropertyChanged();
            }
        }

        private float? _miles;

        public float? Miles
        {
            get
            {
                return _miles;
            }
            set
            {
                _miles = value;
                NotifyPropertyChanged();
            }
        }

        private float? _hours;

        public float? Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
                NotifyPropertyChanged();
            }
        }

        private float? _cycles;

        public float? Cycles
        {
            get
            {
                return _cycles;
            }
            set
            {
                _cycles = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveLogDetail { get; set; }

        private IMessageBus _messageBus;

        public LogDetailPageViewModel(INavigator navigator, IMessageBus messageBus):base(navigator)
        {
            _messageBus = messageBus;

            ExecuteLoadAllDropdowns();

            SaveLogDetail = new Command(SaveLogDetailAsync);
        }

        private void ExecuteLoadAllDropdowns()
        {
            SystemEntities = new ObservableCollection<SystemEntity>
            {
                new SystemEntity
                {
                    SystemId = Guid.NewGuid(),
                    SystemName = "Full Vehicle",
                    IsActive = true
                },
                new SystemEntity
                {
                    SystemId = Guid.NewGuid(),
                    SystemName = "Brakes",
                    IsActive = true
                },
                new SystemEntity
                {
                    SystemId = Guid.NewGuid(),
                    SystemName = "Wheel",
                    IsActive = true
                }
            };

            ComponentEntities = new ObservableCollection<ComponentEntity>
            {
                new ComponentEntity
                {
                    ComponentId = Guid.NewGuid(),
                    ComponentName= "Bolt",
                    IsActive = true
                },
                new ComponentEntity
                {
                    ComponentId = Guid.NewGuid(),
                    ComponentName= "Seat",
                    IsActive = true
                },
                new ComponentEntity
                {
                    ComponentId = Guid.NewGuid(),
                    ComponentName= "handle",
                    IsActive = true
                }
            };

            DetailTypeEntities = new ObservableCollection<DetailTypeEntity>
            {
                new DetailTypeEntity
                {
                    DetailTypeId = Guid.NewGuid(),
                    DetailType = "Road test Detail",
                    IsActive = true
                },
                new DetailTypeEntity
                {
                    DetailTypeId = Guid.NewGuid(),
                    DetailType = "off road test",
                    IsActive = true
                }
            };
        }

        public async void SaveLogDetailAsync()
        {
            _messageBus.Publish(new LogDetailComment
            {
                SystemName = "System",
                ComponentName = "Component",
                Hours = 1.5,
                Miles = 3,
                Cycles = 5,
                CreatedByName = "Narendra",
                CreatedDate = DateTime.Now,
                Comment = "Detail Comment Example. "
            });

            await Navigator.CloseAsync();
        }
    }
}
