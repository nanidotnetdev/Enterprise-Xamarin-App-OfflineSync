using EnterpriseAddLogs.Models;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.Services
{
    public interface ISecurityService
    {
        User CurrentUser { get; set; }

        string CurrentUserId { get; }

        Task LoadUserIdentity();
    }
}
