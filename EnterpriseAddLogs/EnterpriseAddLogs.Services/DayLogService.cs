using System;
using EnterpriseAddLogs.Models;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService :BaseService<DayLog>
    {
		public override string Identifier => "DayLog";

        public override Task<bool> UpsertAsync(DayLog item)
        {
            if (item.IsNew)
            {
                item.DayLogId = Guid.NewGuid();
                return InsertAsync(item);
            }

            return UpdateAsync(item);
        }
    }
}
