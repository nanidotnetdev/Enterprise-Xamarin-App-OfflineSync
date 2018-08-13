namespace EnterpriseAddLogs.Services
{
    using EnterpriseAddLogs.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEngineService
    {
        /// <summary>
        /// Get All Engine Entities.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<EngineEntity>> GetAllEngineEntitiesAsync();
    }
}
