using EnterpriseAddLogs.Models;

namespace EnterpriseAddLogs.Services
{
    public interface ISecurityService
    {
        User CurrentUser { get; set; }

        string CurrentUserId { get; }
    }
}
