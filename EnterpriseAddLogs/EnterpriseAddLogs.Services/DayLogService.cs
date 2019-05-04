using System;
using EnterpriseAddLogs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService : BaseService<DayLog>
    {
		public override string Identifier => "DayLog";

        public override Task<bool> UpsertAsync(DayLog item)
        {
            if (item.DayLogId == Guid.Empty)
            {
                item.DayLogId = Guid.NewGuid();
                return InsertAsync(item);
            }

            return UpdateAsync(item);
        }
    }
}
