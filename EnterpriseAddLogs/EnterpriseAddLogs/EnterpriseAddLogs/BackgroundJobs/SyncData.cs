using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Jobs;

namespace EnterpriseAddLogs.BackgroundJobs
{
    public class SyncData: IJob
    {
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            Debug.WriteLine("ran the job");

            //TODO: Update to sync the data periodically.
            //await AppService.Instance.SyncAsync();
        }
    }
}
