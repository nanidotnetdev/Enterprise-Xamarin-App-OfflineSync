namespace EnterpriseAddLogs.Services
{
    using EnterpriseAddLogs.Models;
    using Microsoft.WindowsAzure.MobileServices;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class LogService : ILogService
    {
        public LogService()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Log>> GetAllLogsAsync()
        {
            try
            {
                IEnumerable<Log> logs = await AzureOfflineService.Instance.logTable.ToEnumerableAsync();

                return new ObservableCollection<Log>(logs);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Log> GetLogAsync(Guid id)
        {
            try
            {
                return await AzureOfflineService.Instance.logTable.LookupAsync(id.ToString());
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task SaveLogAsync(Log log)
        {
            if(log.LogID == Guid.Empty)
            {
                log.LogID = Guid.NewGuid();

                log.CreatedBy = Guid.NewGuid();
                log.CreatedDate = DateTime.Now;

                await AzureOfflineService.Instance.logTable.InsertAsync(log);
            }
            else
            {
                log.UpdatedBy = Guid.NewGuid();
                log.UpdatedDate = DateTime.Now;

                await AzureOfflineService.Instance.logTable.UpdateAsync(log);
            }

            await AzureOfflineService.Instance.SyncAsync().ConfigureAwait(false);
        }
    }
}
