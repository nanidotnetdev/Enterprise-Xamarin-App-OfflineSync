using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAddLogs
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync();

        Task LogoutAsync();
    }
}
