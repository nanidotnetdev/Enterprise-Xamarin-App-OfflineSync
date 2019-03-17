using EnterpriseAddLogs.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.Services
{
    public class AzureOfflineService
    {
        public AzureOfflineService()
        {
            Init();
        }

        private static AzureOfflineService _instance;

        public static AzureOfflineService Instance
        {
            get
            {
                return _instance ?? (_instance = new AzureOfflineService());
            }
        }

        MobileServiceClient _client;

        public MobileServiceClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new MobileServiceClient(ServiceConstants.Urls.AzureBackendURL);
                }
                
                return _client;
            }
        }

        public IMobileServiceSyncTable<Log> logTable;
        public IMobileServiceSyncTable<DayLog> dayLogTable;

        private NetworkAccess _networkAccess;

        public async Task Init()
        {
            //already Initialized.
            if (Client.SyncContext.IsInitialized)
                return;

            // Create a reference to the local sqlite store
            var store = new MobileServiceSQLiteStore("sqllitedb.db");

            // Define the database schema
            store.DefineTable<Log>();
            store.DefineTable<DayLog>();

            // Actually create the store and update the schema
            await Client.SyncContext.InitializeAsync(store);

            logTable = Client.GetSyncTable<Log>();
            dayLogTable = Client.GetSyncTable<DayLog>();

            _networkAccess = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        /// <summary>
        /// Sync with Server.
        /// </summary>
        /// <returns></returns>
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                //use xamarin crossconnectivity to make it work -- better to use individual plugin as it has more options available.
                //if (!(await Connectivity.Current.IsRemoteReachable(client.MobileAppUri.Host, 443)))
                //{
                //    Debug.WriteLine($"Cannot connect to {client.MobileAppUri} right now - offline");
                //    return;
                //}

                await Init();

                //when offline 
                if (_networkAccess != NetworkAccess.Internet)
                    return;

                await Client.SyncContext.PushAsync();

                //pass null as query string name to pull all the data
                //pass name to pull only latest 50 records.- incremental sync
                await dayLogTable.PullAsync("GetAllDayLog", dayLogTable.CreateQuery());
                await logTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "GetLogs",
                    logTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                Crashes.TrackError(exc);

                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }
                }
            }
        }

        /// <summary>
        /// on network change trigger the sync async
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            _networkAccess = e.NetworkAccess;

            var profiles = e.ConnectionProfiles;

            SyncAsync();
        }
    }
}
