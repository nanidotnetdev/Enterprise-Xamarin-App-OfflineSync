using System.Collections.Generic;
using System.Linq;
using Autofac;
using Plugin.Jobs;

namespace EnterpriseAddLogs.BackgroundJobs
{
    public class JobBuilder
    {
        static IContainer _jobContainer;

        /// <summary>
        /// 
        /// </summary>
        static void InitJobContainer()
        {
            CrossJobs.ResolveJob = ResolveJob;

            if (_jobContainer != null)
                return;

            // register the jobs here as well as any dependencies they have
            var builder = new ContainerBuilder();
            builder.RegisterType<SyncData>().As<IJob>().SingleInstance();
            _jobContainer = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public static IJob ResolveJob(JobInfo jobInfo)
        {
            InitJobContainer();

            if (_jobContainer.TryResolve(out IEnumerable<IJob> list))
            {
                var job = list.FirstOrDefault(x => x.GetType() == jobInfo.Type);
                return job;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ScheduleJobs()
        {
            var job = new JobInfo
            {
                Name = "SyncDataJob",
                Type = typeof(SyncData),
                Repeat = true,
                BatteryNotLow = true,
                DeviceCharging = false,
                RequiredNetwork = NetworkType.Any
            };

            CrossJobs.Current.Schedule(job).GetAwaiter().GetResult();
        }
    }
}
