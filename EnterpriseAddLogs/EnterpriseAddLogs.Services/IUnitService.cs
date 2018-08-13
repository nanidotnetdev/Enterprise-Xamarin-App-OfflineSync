namespace EnterpriseAddLogs.Services
{
    using EnterpriseAddLogs.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUnitService
    {
        /// <summary>
        /// get All Unit Entities.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<UnitEntity>> GetAllUnitEntitiesAsync();
    }
}
