namespace EnterpriseAddLogs.Services
{
    using EnterpriseAddLogs.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LogEntity> GetLogAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICollection<LogEntity>> GetAllLogsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Log"></param>
        /// <returns></returns>
        Task<LogEntity> SaveLogAsync(LogEntity log);

    }
}
