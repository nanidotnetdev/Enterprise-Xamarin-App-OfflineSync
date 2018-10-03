using EnterpriseAddLogs.Helpers;
using EnterpriseAddLogs.Messaging;
using EnterpriseAddLogs.Models;
using EnterpriseAddLogs.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        private ProductGroupEntity _selectedProductGroup { get; set; }

        public ProductGroupEntity SelectedProductGroup
        {
            get {
                return _selectedProductGroup;
            }
            set{
                _selectedProductGroup = value;
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

        private UnitEntity _selectedUnitNumber { get; set; }

        public UnitEntity SelectedUnitNumber
        {
            get
            {
                return _selectedUnitNumber;
            }
            set
            {
                _selectedUnitNumber = value;
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

        private UserEntity _selectedAssignedDriver { get; set; }

        public UserEntity SelectedAssignedDriver
        {
            get
            {
                return _selectedAssignedDriver;
            }
            set
            {
                _selectedAssignedDriver = value;
                NotifyPropertyChanged();
            }
        }

        private UserEntity _selectedEnteredBy { get; set; }

        public UserEntity SelectedEnteredBy
        {
            get
            {
                return _selectedEnteredBy;
            }
            set
            {
                _selectedEnteredBy = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<LogTypeEntity> _logTypeEntities { get; set; }
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

        private LogTypeEntity _selectedLogType { get; set; }

        public LogTypeEntity SelectedLogType
        {
            get
            {
                return _selectedLogType;
            }
            set
            {
                _selectedLogType = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _logCreatedDate { get; set; }

        private DateTime LogCreatedDate
        {
            get
            {
                return _logCreatedDate;
            }
            set
            {
                _logCreatedDate = value;
                NotifyPropertyChanged();
            }
        }

        private string _newLogComment;

        public string NewLogComment
        {
            get
            {
                return _newLogComment;
            }
            set
            {
                _newLogComment = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand AddNewLogCommand { get; set; }

        public ICommand SaveLogCommand { get; set; }

        public ICommand AddNewDetailCommand { get; set; }

        private IList<CommentEntity> logComments { get; set; }

        private ObservableCollection<CommentEntity> _logComments { get; set; }

        public ObservableCollection<CommentEntity> LogComments
        {
            get
            {
                return _logComments;
            }
            set
            {
                _logComments = value;
                NotifyPropertyChanged();
            }
        }

        private IList<LogDetailComment> logDetailComments { get; set; }

        private ObservableCollection<LogDetailComment> _logDetailComments { get; set; }

        public ObservableCollection<LogDetailComment> LogDetailComments
        {
            get
            {
                return _logDetailComments;
            }
            set
            {
                _logDetailComments = value;
                NotifyPropertyChanged();
            }
        }

        private bool _addNewCommentToggle { get; set; }

        public bool AddNewCommentToggle
        {
            get
            {
                return _addNewCommentToggle;
            }
            set
            {
                _addNewCommentToggle = value;
                NotifyPropertyChanged();
            }
        }

        private IMessageBus _messageBus;

        public ICommand AddNewCommentCommand { get; set; }

        public LogCreatePageViewModel(IUserService userService, IUnitService unitService,
            IProductGroupService productGroupService, ILogService logService,
            ILogTypeService logTypeService, INavigator navigator, IMessageBus messageBus): base(navigator)
        {

            IsBusy = true;

            _userService = userService;
            _unitService = unitService;
            _productGroupService = productGroupService;
            _logService = logService;
            _logTypeService = logTypeService;
            _messageBus = messageBus;

            _productGroupEntities = new ObservableCollection<ProductGroupEntity>();
            _unitEntities = new ObservableCollection<UnitEntity>();
            _userEntities = new ObservableCollection<UserEntity>();
            _logTypeEntities = new ObservableCollection<LogTypeEntity>();
            _logDetailComments = new ObservableCollection<LogDetailComment>();
            logDetailComments = new List<LogDetailComment>();
            logComments = new List<CommentEntity>();

            LogCreatedDate = DateTime.Now;

            ExecuteLoadStaticDropdownsAsync();

            SaveLogCommand = new Command(SaveLogAsync);
            AddNewLogCommand = new Command(SaveLogComment);
            AddNewDetailCommand = new Command(AddNewDetailCommentAsync);

            messageBus.Subscribe<LogDetailComment>(async message =>
            {
                var m = message;
                logDetailComments.Add(message);
                LogDetailComments = new ObservableCollection<LogDetailComment>(logDetailComments);
            });
        }

        private async void AddNewDetailCommentAsync()
        {
            await Navigator.NavigateToViewModelAsync<LogDetailPageViewModel>();
        }

        private async void SaveLogComment()
        {
            IsBusy = true;

            var newComment = new CommentEntity
            {
                Comment = NewLogComment,
                CommentId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedByName = "Narendra"
            };

            //TODO: update to use the Api.
            logComments.Add(newComment);
            NewLogComment = string.Empty;

            LogComments = new ObservableCollection<CommentEntity>(logComments);

            IsBusy = false;
        }

        private async void SaveLogAsync()
        {
            IsBusy = true;

            var logentity = new Log
            {
                UnitID = SelectedUnitNumber.UnitID,
                AssignedDriver = SelectedAssignedDriver.UserId,
                LogTypeID = SelectedLogType.LogTypeID,
                ProductGroupID = SelectedProductGroup.ProductGroupId,
                EnteredBy = SelectedEnteredBy.UserId,
                EnteredDate = DateTime.Now
            };

            await _logService.SaveLogAsync(logentity);
            IsBusy = false;

            await Navigator.CloseAsync();

            await Navigator.NavigateToViewModelAsync<LogIndexPageViewModel>();
        }

        private async Task ExecuteLoadStaticDropdownsAsync()
        {
            var t1 = ExecuteLoadProductGroupEntities();
            var t2 = ExecuteLoadUnitEntities();
            var t3 = ExecuteLoadUserEntities();
            var t4 = ExecuteLoadLogTypeEntities();
            var t5 = ExecuteLoadCommentEntities();

            await Task.WhenAll(t1, t2, t3, t4);

            IsBusy = false;
        }

        private async Task ExecuteLoadCommentEntities()
        {
            logComments = new List<CommentEntity>
            {
                new CommentEntity
                {
                    CommentId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedByName = "Narendra",
                    Comment ="The break rod was broken. need to replace. Find the order and replace."
                },
                new CommentEntity
                {
                    CommentId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedByName = "Kavan",
                    Comment ="The break rod was broken. need to replace. Find the order and replace."
                }
            };

            LogComments = new ObservableCollection<CommentEntity>(logComments);
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
                var id = item.UserId;
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
            var selLog = parameter;

            return base.OnNavigatedToAsync(parameter);
        }
    }
}
