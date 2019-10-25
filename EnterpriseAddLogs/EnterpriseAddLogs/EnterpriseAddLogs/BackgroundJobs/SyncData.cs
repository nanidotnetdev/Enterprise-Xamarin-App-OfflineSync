using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EnterpriseAddLogs.Services;
using Plugin.Jobs;
using Plugin.LocalNotifications;

namespace EnterpriseAddLogs.BackgroundJobs
{
    public class SyncData: IJob
    {
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            CrossLocalNotifications.Current.Show("Enterprise Add Logs", "BackGround Job Ran!!");

            //TODO: Update to sync the data periodically.
            //await AppService.Instance.SyncAsync();
        }
    }
}
