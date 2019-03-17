using EnterpriseAddLogs.Models;
using Microsoft.AppCenter.Crashes;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService : IDayLogService
    {
        
        public DayLogService()
        {
        }

        public async Task<DayLog> GetById(Guid Id)
        {
            try
            {
                var Query = AzureOfflineService.Instance.dayLogTable.CreateQuery()
                    .Where(l => l.DayLogId == Id);

                IEnumerable<DayLog> res = await AzureOfflineService.Instance.dayLogTable.Where(l => l.DayLogId == Id)
                    .ToEnumerableAsync();

                return res.FirstOrDefault();
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<DayLog>> GetDayLogs()
        {
            try
            {
                await AzureOfflineService.Instance.SyncAsync();

                IEnumerable<DayLog> dayLogs = await AzureOfflineService.Instance.dayLogTable
                    .OrderByDescending(l => l.DateLogged).ToEnumerableAsync();

                return new ObservableCollection<DayLog>(dayLogs);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Crashes.TrackError(msioe);

                //Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);

                //Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayLog"></param>
        /// <returns></returns>
        public async Task SaveDayLog(DayLog dayLog)
        {
            try
            {
                if (dayLog.DayLogId == Guid.Empty)
                {
                    dayLog.DayLogId = Guid.NewGuid();

                    await AzureOfflineService.Instance.dayLogTable.InsertAsync(dayLog);
                }
                else
                {
                    await AzureOfflineService.Instance.dayLogTable.UpdateAsync(dayLog);
                }

                await AzureOfflineService.Instance.SyncAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
