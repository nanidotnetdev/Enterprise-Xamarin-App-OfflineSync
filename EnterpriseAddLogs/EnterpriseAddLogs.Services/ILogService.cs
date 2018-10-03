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
        Task<Log> GetLogAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICollection<Log>> GetAllLogsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Log"></param>
        /// <returns></returns>
        Task<Log> SaveLogAsync(Log log);

    }
}
