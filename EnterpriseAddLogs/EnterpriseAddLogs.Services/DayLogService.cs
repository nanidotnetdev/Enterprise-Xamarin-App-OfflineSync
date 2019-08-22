using EnterpriseAddLogs.Models;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService :BaseService<DayLog>, IDayLogService
    {
		public override string Identifier => "DayLog";
    }
}
