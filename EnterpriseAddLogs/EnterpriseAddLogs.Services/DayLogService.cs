using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EnterpriseAddLogs.Models;
using Microsoft.AppCenter.Crashes;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Connectivity;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService :BaseService<DayLog>, IDayLogService
    {
        private readonly ISecurityService _securityService;

        public DayLogService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public override string Identifier => "DayLog";

        /// <summary>
        /// pull the day logs belong to the user only.
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> PullLatestAsync(IMobileServiceTableQuery<DayLog> query = null)
        {
            query = query ?? Table.CreateQuery()
                        .Where(l => l.CreatedBy == _securityService.CurrentUserId);

            return await base.PullLatestAsync(query);
        }
    }
}
