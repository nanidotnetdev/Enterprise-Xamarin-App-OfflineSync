using System.Threading.Tasks;
using EnterpriseAddLogs.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace EnterpriseAddLogs.Services
{
    public class UserService: BaseService<User>, IUserService
    {
        private readonly ISecurityService _securityService;

        public UserService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public override string Identifier => "User";
    }
}
