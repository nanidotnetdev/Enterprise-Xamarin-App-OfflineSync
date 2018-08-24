using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.ViewModels
{
    public class LogCreatePageViewModel : PageViewModel
    {
        private readonly IUserService _userService;

        private readonly IUnitService _unitService;

        private readonly IProductGroupService _productGroupService;

        private readonly ILogService _logService;

        private readonly ILogTypeService _logTypeService;

        private static ObservableCollection<ProductGroupEntity> _productGroupEntities { get; set; }

        public ObservableCollection<ProductGroupEntity> ProductGroupEntities
        {
            get
            {
                return _productGroupEntities;
            }
            set
            {
                _productGroupEntities = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<UnitEntity> _unitEntities;

        public ObservableCollection<UnitEntity> UnitEntities
        {
            get
            {
                return _unitEntities;
            }
            set
            {
                _unitEntities = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserEntity> _userEntities { get; set; }
        public ObservableCollection<UserEntity> UserEntities
        {
            get
            {
                return _userEntities;
            }
            set
            {
                _userEntities = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<LogTypeEntity> _logTypeEntities { get; set; }
        public ObservableCollection<LogTypeEntity> LogTypeEntities
        {
            get
            {
                return _logTypeEntities;
            }
            set
            {
                _logTypeEntities = value;
                NotifyPropertyChanged();
            }
        }

        public LogCreatePageViewModel(IUserService userService, IUnitService unitService,
            IProductGroupService productGroupService, ILogService logService,
            ILogTypeService logTypeService, INavigator navigator): base(navigator)
        {
            _userService = userService;
            _unitService = unitService;
            _productGroupService = productGroupService;
            _logService = logService;
            _logTypeService = logTypeService;

            _productGroupEntities = new ObservableCollection<ProductGroupEntity>();
            _unitEntities = new ObservableCollection<UnitEntity>();
            _userEntities = new ObservableCollection<UserEntity>();
            _logTypeEntities = new ObservableCollection<LogTypeEntity>();

            ExecuteLoadStaticDropdownsAsync();

        }

        private async Task ExecuteLoadStaticDropdownsAsync()
        {
            var t1 = ExecuteLoadProductGroupEntities();
            var t2 = ExecuteLoadUnitEntities();
            var t3 = ExecuteLoadUserEntities();
            var t4 = ExecuteLoadLogTypeEntities();

            await Task.WhenAll(t1, t2, t3, t4);
        }

        private async Task ExecuteLoadProductGroupEntities()
        {
            ICollection<ProductGroupEntity> Entities = await _productGroupService.GetAllProductgroupEntitiesAsync();

            foreach (var item in Entities)
            {
                ProductGroupEntities.Add(item);
            }
        }

        private async Task ExecuteLoadUnitEntities()
        {
            ICollection<UnitEntity> Entities = await _unitService.GetAllUnitEntitiesAsync();

            foreach (var item in Entities)
            {
                UnitEntities.Add(item);
            }
        }

        private async Task ExecuteLoadUserEntities()
        {
            ICollection<UserEntity> Entities = await _userService.GetAllUsersAsync();
            foreach (var item in Entities)
            {
                UserEntities.Add(item);
            }
        }

        private async Task ExecuteLoadLogTypeEntities()
        {
            ICollection<LogTypeEntity> Entities = await _logTypeService.GetAllLogTypeEntitiesAsync();

            foreach (var item in Entities)
            {
                LogTypeEntities.Add(item);
            }
        }

        public override Task OnNavigatedToAsync(object parameter = null)
        {
            return base.OnNavigatedToAsync(parameter);
        }
    }
}
