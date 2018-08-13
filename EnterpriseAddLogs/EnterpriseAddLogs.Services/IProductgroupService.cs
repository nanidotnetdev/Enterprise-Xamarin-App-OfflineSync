namespace EnterpriseAddLogs.Services
{
    using EnterpriseAddLogs.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductGroupService
    {
        /// <summary>
        /// Get All product group Entities.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<ProductGroupEntity>> GetAllProductgroupEntitiesAsync();
    }
}
