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

        /// <summary>
        /// pull the logged in user only.
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> PullLatestAsync(IMobileServiceTableQuery<User> query = null)
        {
            query = query ?? Table.CreateQuery()
                        .Where(u => u.Id == _securityService.CurrentUserId);

            return await base.PullLatestAsync(query);
        }
    }
}
