namespace EnterpriseAddLogs.Services
{
    using EnterpriseAddLogs.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILogTypeService
    {
        /// <summary>
        /// Get All LogType Entities.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<LogTypeEntity>> GetAllLogTypeEntitiesAsync();
    }
}
