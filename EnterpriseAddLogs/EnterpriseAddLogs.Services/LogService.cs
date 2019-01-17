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
                await this.SyncAsync();

            }
            catch (Exception ex)
            {

            }

            try
            {
                //var table = logTable.PullAsync();
                ICollection<Log> logs = await logTable.ToCollectionAsync();

                return logs;
            }
            catch(Exception ex)
            {

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
            //using (var client = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format(ServiceConstants.Urls.GetLogEntity, id)))
            //    {

            //        AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

            //        HttpResponseMessage response = null;

            //        using (var cancellationSource = new CancellationTokenSource())
            //        {
            //            cancellationSource.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

            //            response = await client.SendAsync(request, cancellationSource.Token).ConfigureAwait(false);
            //        }

            //        response.EnsureSuccessStatusCode();

            //        var jsonString = await response.Content.ReadAsStringAsync();

            //        return JsonConvert.DeserializeObject<Log>(jsonString);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<Log> SaveLogAsync(Log log)
        {
            //using (var client = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(HttpMethod.Post, ServiceConstants.Urls.SaveLogEntity))
            //    {
            //        AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

            //        HttpResponseMessage response = null;

            //        //Instead of POST Async add content in the request.
            //        request.Content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");

            //        using (var cancellationToken = new CancellationTokenSource())
            //        {
            //            cancellationToken.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

            //            response = await client.SendAsync(request, cancellationToken.Token)
            //                .ConfigureAwait(false);

            //        }

            //        response.EnsureSuccessStatusCode();

            //        var jsonString = await response.Content.ReadAsStringAsync();

            //        return JsonConvert.DeserializeObject<Log>(jsonString);
            //    }
            //}
        }
    }
}
