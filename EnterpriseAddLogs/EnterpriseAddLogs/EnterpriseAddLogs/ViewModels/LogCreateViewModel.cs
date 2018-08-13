using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.ViewModels
{
    public class LogCreateViewModel: PageViewModel
    {
        private readonly IUserService _userService;

        private readonly IUnitService _unitService;

        private readonly IProductGroupService _productGroupService;

        private readonly ILogService _logService;

        private readonly ILogTypeService _logTypeService;

        private static List<ProductGroupEntity> _productGroupEntities;

        private static List<UnitEntity> _unitEntities;

        private static List<UserEntity> _userEntities;

        private readonly List<LogTypeEntity> _logTypeEntities;

        public LogCreateViewModel(IUserService userService, IUnitService unitService, 
            IProductGroupService productGroupService, ILogService logService, ILogTypeService logTypeService)
        {
            _userService = userService;
            _unitService = unitService;
            _productGroupService = productGroupService;
            _logService = logService;
            _logTypeService = logTypeService;

            _productGroupEntities = new List<ProductGroupEntity>();
            _unitEntities = new List<UnitEntity>();
            _userEntities = new List<UserEntity>();
            _logTypeEntities = new List<LogTypeEntity>();

            ExecuteLoadStaticDropdowns();
        }

        private async void ExecuteLoadStaticDropdowns()
        {

            var t1 = ExecuteLoadProductGroupEntities();
            var t2 = ExecuteLoadUnitEntities();
            var t3 = ExecuteLoadUserEntities();
            var t4 = ExecuteLoadLogTypeEntities();

            await Task.WhenAll(t1, t2, t3, t4);
        }

        private async Task ExecuteLoadProductGroupEntities()
        {
            ICollection<ProductGroupEntity> productGroupEntities = await _productGroupService.GetAllProductgroupEntitiesAsync();

            _productGroupEntities.AddRange(productGroupEntities);
        }

        private async Task ExecuteLoadUnitEntities()
        {
            ICollection<UnitEntity> unitEntities = await _unitService.GetAllUnitEntitiesAsync();

            _unitEntities.AddRange(unitEntities);
        }

        private async Task ExecuteLoadUserEntities()
        {
            ICollection<UserEntity> userEntities = await _userService.GetAllUsersAsync();
            _userEntities.AddRange(userEntities);
        }

        private async Task ExecuteLoadLogTypeEntities()
        {
            ICollection<LogTypeEntity> logTypeEntities = await _logTypeService.GetAllLogTypeEntitiesAsync();
            _logTypeEntities.AddRange(logTypeEntities);
        }

    }
}
