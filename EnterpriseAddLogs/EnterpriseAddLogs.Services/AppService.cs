using System.Collections.Generic;
using EnterpriseAddLogs.Models;
using Microsoft.AppCenter.Crashes;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.Services
{
    public class AppService
    {
        public AppService()
        {
            Init();
        }

        private static AppService _instance;

        public static AppService Instance => _instance ?? (_instance = new AppService());

        MobileServiceClient _client;

        public MobileServiceClient Client => _client ??
                                             (_client = new MobileServiceClient(ServiceConstants.Urls.AzureBackendURL, 
                                                 new Xamarin.Android.Net.AndroidClientHandler()));

        public UserIdentity UserIdentity { get; set; }

        private NetworkAccess _networkAccess;

        public async Task Init()
        {
            //already Initialized.
            if (Client.SyncContext.IsInitialized)
                return;

            // Create a reference to the local sqlite store
            var store = new MobileServiceSQLiteStore("localdb14235.db");

            // Define the database schema
            store.DefineTable<DayLog>();
            store.DefineTable<User>();

            // Actually create the store and update the schema
            await Client.SyncContext.InitializeAsync(store);

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
                if (!await CrossConnectivity.Current.IsRemoteReachable(_client.MobileAppUri.Host, 443))
                {
                    Debug.WriteLine($"Cannot connect to {_client.MobileAppUri} right now - offline");
                    return;
                }

                await Init();

                //new base service approach
                //var list = new List<Task<bool>>();
                ////TODO:add all sync methods
                //await Task.WhenAll(list).ConfigureAwait(false);
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
