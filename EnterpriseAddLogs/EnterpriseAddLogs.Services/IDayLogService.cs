using EnterpriseAddLogs.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.Services
{
    public interface IDayLogService
    {
        Task<DayLog> GetById(string id);

        Task SaveDayLog(DayLog dayLog);

        Task<ICollection<DayLog>> GetDayLogs();
    }
}
