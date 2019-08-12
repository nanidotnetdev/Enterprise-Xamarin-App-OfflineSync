namespace EnterpriseAddLogs.Droid
{
    using System;
    using Android.App;
    using Android.Runtime;
    using EnterpriseAddLogs.BackgroundJobs;
    using Plugin.Jobs;

    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            CrossJobs.ResolveJob = JobBuilder.ResolveJob;
            CrossJobs.Init(this);
            base.OnCreate();
        }
    }
}