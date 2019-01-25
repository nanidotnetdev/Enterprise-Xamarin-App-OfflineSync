namespace EnterpriseAddLogs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EnterpriseAddLogs.Models;
    using Microsoft.WindowsAzure.MobileServices;
    using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
    using Microsoft.WindowsAzure.MobileServices.Sync;
    using Newtonsoft.Json;

    public class LogService : AzureService, ILogService
    {

        //static LogService logService = new LogService();
        //private MobileServiceClient client;
        //IMobileServiceSyncTable<Log> logTable;

        public LogService()
        {
            try
            {

                //this.client = new MobileServiceClient(ServiceConstants.Urls.AzureBackendURL);
                
                
            }
            catch (Exception ex)
            {

            }
        }

        

        //public async Task SyncAsync()
        //{
        //    ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

        //    //try
        //    //{
        //    //    var logentity = new Log
        //    //    {
        //    //        UnitID = Guid.NewGuid(),
        //    //        AssignedDriver = Guid.NewGuid(),
        //    //        LogTypeID = Guid.NewGuid(),
        //    //        ProductGroupID = Guid.NewGuid(),
        //    //        EnteredBy = Guid.NewGuid(),
        //    //        EnteredDate = DateTime.Now,
        //    //        LogID = Guid.NewGuid(),
        //    //        CreatedDate = DateTime.Now,
        //    //        CreatedBy = Guid.Parse("E4C6D172-3D14-4539-9FEE-306B081DC3DB")
        //    //    };

        //    //    this.logTable = client.GetSyncTable<Log>();

        //    //    await logTable.InsertAsync(logentity);

        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //}

        //    try
        //    {
        //        var store = new MobileServiceSQLiteStore("localstorepdvs.db");
        //        store.DefineTable<Log>();
        //        await this.client.SyncContext.InitializeAsync(store);
        //        this.logTable = client.GetSyncTable<Log>();

        //        if (client.SyncContext.IsInitialized)
        //        {
        //            string queryName = $"incsync_{typeof(Log).Name}";
        //            await this.logTable.PullAsync(queryName, logTable.CreateQuery());

        //        }
        //    }
        //    catch (MobileServicePushFailedException exc)  
        //    {
        //        if (exc.PushResult != null)
        //        {
        //            syncErrors = exc.PushResult.Errors;
        //        }
        //    }
        //    try
        //    {
        //        await this.client.SyncContext.PushAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    // Simple error/conflict handling. A real application would handle the various errors like network conditions,
        //    // server conflicts and others via the IMobileServiceSyncHandler.
        //    if (syncErrors != null)
        //    {
        //        foreach (var error in syncErrors)
        //        {
        //            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
        //            {
        //                //Update failed, reverting to server's copy.
        //                await error.CancelAndUpdateItemAsync(error.Result);
        //            }
        //            else
        //            {
        //                // Discard local change.
        //                await error.CancelAndDiscardItemAsync();
        //            }

        //            Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
        //        }
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Log>> GetAllLogsAsync()
        {
            try
            {
                await SyncAsync();

                IEnumerable<Log> logs = await logTable.ToEnumerableAsync();

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
                await SyncAsync();

                IEnumerable<Log> logs = await logTable.ToEnumerableAsync();

                return await logTable.LookupAsync(id.ToString());
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

                await logTable.InsertAsync(log);
            }
            else
            {
                log.UpdatedBy = Guid.NewGuid();
                log.UpdatedDate = DateTime.Now;

                await logTable.UpdateAsync(log);

            }

            SyncAsync();
        }
    }
}
