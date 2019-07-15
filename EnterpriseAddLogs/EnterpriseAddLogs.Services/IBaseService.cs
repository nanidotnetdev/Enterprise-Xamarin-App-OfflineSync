using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAddLogs.Models;

namespace EnterpriseAddLogs.Services
{
    public interface IBaseService<T> where T:BaseModel
    {
        Task<IList<T>> GetItemsAsync(bool forceRefresh = false);

        Task<T> GetItemAsync(string id, bool forceRefresh = false);

        Task<bool> UpsertAsync(T item);

        Task<bool> InsertAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> RemoveAsync(T item);

        Task<bool> PullLatestAsync();

        Task<bool> SyncAsync();
    }
}
