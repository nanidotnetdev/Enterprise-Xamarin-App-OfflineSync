using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAddLogs.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace EnterpriseAddLogs.Services
{
    public class DayLogService : AzureService, IDayLogService
    {

        public DayLogService()
        {
        }

        public async Task<ICollection<DayLog>> GetDayLogs()
        {
            try
            {
                await SyncAsync();

                IEnumerable<DayLog> dayLogs = await dayLogTable.ToEnumerableAsync();

                return new ObservableCollection<DayLog>(dayLogs);
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

        public async Task SaveDayLog(DayLog dayLog)
        {
            if(dayLog.DayLogId == Guid.Empty)
            {
                dayLog.DayLogId = Guid.NewGuid();

                await dayLogTable.InsertAsync(dayLog);
            }
            else
            {
                await dayLogTable.UpdateAsync(dayLog);
            }

            SyncAsync();
        }
    }
}
