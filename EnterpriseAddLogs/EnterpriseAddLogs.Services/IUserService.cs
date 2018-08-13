namespace EnterpriseAddLogs.Services
{
    using System.Net;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using EnterpriseAddLogs.Models;

    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserEntity> GetUserAsync(Guid userId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICollection<UserEntity>> GetAllUsersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserEntity> SaveUserAsync(UserEntity user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HttpStatusCode> DeleteUserAsync(Guid id);
    }
}
