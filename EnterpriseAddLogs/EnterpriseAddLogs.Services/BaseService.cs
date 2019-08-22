using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;
using System.Text;
using EnterpriseAddLogs.Models;
using Microsoft.AppCenter.Crashes;

namespace EnterpriseAddLogs.Services
{
	public class BaseService<T> : IBaseService<T> where T: BaseModel
	{
		public virtual string Identifier => "Items";

		IMobileServiceSyncTable<T> table;
		public IMobileServiceSyncTable<T> Table => table ?? (table = AppService.Instance.Client.GetSyncTable<T>());

        public void DropTable()
		{
			table = null;
		}

		public virtual async Task<IList<T>> GetItemsAsync(bool forceRefresh = false)
		{
			if(forceRefresh)
				await PullLatestAsync().ConfigureAwait(false);

			var enu = await Table.ToEnumerableAsync().ConfigureAwait(false);
			return enu.ToList();
		}

		public virtual async Task<T> GetItemAsync(string id, bool forceRefresh = false)
		{
			if(forceRefresh)
				await PullLatestAsync().ConfigureAwait(false);

			var item = await Table.LookupAsync(id).ConfigureAwait(false);
			return item;
		}

		public virtual async Task<bool> UpsertAsync(T item)
        {
            if(item.Id == null)
			{
				return await InsertAsync(item);
			}

            return await UpdateAsync(item);
        }

		public virtual async Task<bool> InsertAsync(T item)
        {
            item.Id = Guid.NewGuid().ToString();
			await Table.InsertAsync(item);
			var success = await SyncAsync().ConfigureAwait(false);

			if(success)
			{
				var updated = await GetItemAsync(item.Id, false).ConfigureAwait(false);
				item.Version = updated.Version;
				item.UpdatedAt = updated.UpdatedAt;
			}

			return success;
		}

		public virtual async Task<bool> UpdateAsync(T item)
		{
			try
			{
				await Table.UpdateAsync(item).ConfigureAwait(false);
				var success = await SyncAsync().ConfigureAwait(false);
				var updated = await GetItemAsync(item.Id, false).ConfigureAwait(false);

				item.Version = updated.Version;
				item.UpdatedAt = updated.UpdatedAt;

				return success;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}

		public virtual async Task<bool> RemoveAsync(T item)
		{
			await Table.DeleteAsync(item).ConfigureAwait(false);
			var success = await SyncAsync().ConfigureAwait(false);
			return success;
		}

		public virtual async Task<bool> PullLatestAsync(IMobileServiceTableQuery<T> query = null)
		{
			if(!CrossConnectivity.Current.IsConnected)
			{
				Debug.WriteLine("Unable to pull items, we are offline");
				return false;
			}
			try
            {
                query = query ?? Table.CreateQuery();

				//Pull down any content from the server that doesn't exist locally and add it to the local database
				await Table.PullAsync($"all{Identifier}", query).ConfigureAwait(false);
			}
			catch(Exception ex)
			{
                Crashes.TrackError(ex);
				Debug.WriteLine($"Pull sync error for {Identifier}\n" + ex);
				return false;
			}
			return true;
		}


		public async Task<bool> SyncAsync()
		{
			if(!CrossConnectivity.Current.IsConnected)
			{
				Debug.WriteLine("Unable to sync items, we are offline");
				return false;
			}
			try
			{
				await AppService.Instance.Client.SyncContext.PushAsync();

				if(!(await PullLatestAsync()))
					return false;
			}
			catch(MobileServicePushFailedException ex)
			{
				if(ex.PushResult?.Errors != null)
				{
					foreach(var error in ex.PushResult.Errors)
					{
						if(error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
						{
							//Update failed, reverting to server's copy
							await error.CancelAndUpdateItemAsync(error.Result);
						}
						else
						{
							//Discard local change
							await error.CancelAndDiscardItemAsync();
						}

						var sb = new StringBuilder();
						foreach(var v in error.Result)
							sb.AppendLine(v.Value.ToString());

                        Debug.WriteLine($"Error executing sync operation. Item: {error.TableName} ({error.Item["id"]}). Operation discarded - {sb.ToString()}\n\n");
					}
				}

                Crashes.TrackError(ex);
                Debug.WriteLine("Unable to sync\n" + ex);
				return false;
			}

			return true;
		}
	}
}