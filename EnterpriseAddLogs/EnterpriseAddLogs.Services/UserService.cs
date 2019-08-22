using EnterpriseAddLogs.Models;

namespace EnterpriseAddLogs.Services
{
    public class UserService: BaseService<User>, IUserService
    {
        public override string Identifier => "User";
    }
}
