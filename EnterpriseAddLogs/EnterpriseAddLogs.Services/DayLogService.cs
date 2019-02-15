using EnterpriseAddLogs.Models;
using Microsoft.AppCenter.Crashes;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService : IDayLogService
    {
        
        public DayLogService()
        {
        }

        public async Task<DayLog> GetById(string id)
        {
            try
            {
                return await AzureOfflineService.dayLogTable.LookupAsync(id);
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
                await AzureOfflineService.SyncAsync();

                IEnumerable<DayLog> dayLogs = await AzureOfflineService.dayLogTable.OrderByDescending(l => l.CreatedAt).ToEnumerableAsync();

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

                    await AzureOfflineService.dayLogTable.InsertAsync(dayLog);
                }
                else
                {
                    await AzureOfflineService.dayLogTable.UpdateAsync(dayLog);
                }

                AzureOfflineService.SyncAsync();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
