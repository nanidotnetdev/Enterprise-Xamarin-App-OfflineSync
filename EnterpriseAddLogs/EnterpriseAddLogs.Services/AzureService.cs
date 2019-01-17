using EnterpriseAddLogs.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.Services
{
    public class AzureService
    {
        private static AzureService azureService = new AzureService();

        private NetworkAccess _networkAccess;

        protected MobileServiceClient client;
        protected IMobileServiceSyncTable<Log> logTable;
        protected IMobileServiceSyncTable<DayLog> dayLogTable;

        public static AzureService DefaultManager
        {
            get
            {
                return azureService;
            }
            private set
            {
                azureService = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public AzureService()
        {

            _networkAccess = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            InitializeAsync();
        }

        #region Offline Sync Initialization
        async Task InitializeAsync()
        {
            client = new MobileServiceClient(ServiceConstants.Urls.AzureBackendURL);

            // Short circuit - local database is already initialized
            if (client.SyncContext.IsInitialized)
                return;

            // Create a reference to the local sqlite store
            var store = new MobileServiceSQLiteStore("OfflineSyncdb2.db");

            // Define the database schema
            store.DefineTable<Log>();
            store.DefineTable<DayLog>();

            this.logTable = client.GetSyncTable<Log>();
            this.dayLogTable = client.GetSyncTable<DayLog>();

            // Actually create the store and update the schema
            await client.SyncContext.InitializeAsync(store);
        }
        #endregion

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

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await InitializeAsync();

                //use xamarin crossconnectivity to make it work -- better to use individual plugin as it has more options available.
                //if (!(await Connectivity.Current.IsRemoteReachable(client.MobileAppUri.Host, 443)))
                //{
                //    Debug.WriteLine($"Cannot connect to {client.MobileAppUri} right now - offline");
                //    return;
                //}

                //when offline 
                if (_networkAccess != NetworkAccess.Internet)
                    return;



                await this.client.SyncContext.PushAsync();

                //pass null as query string name to pull all the data
                //pass name to pull only latest 50 records.
                await this.dayLogTable.PullAsync(null, this.dayLogTable.CreateQuery());
                await this.logTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    null,
                    this.logTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
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

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
