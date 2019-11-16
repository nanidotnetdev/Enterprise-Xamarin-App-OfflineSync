using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Fingerprint;
using EnterpriseAddLogs.Messaging;
using Autofac;
using Acr.UserDialogs;
using Android.Content;
using Android.Views.Accessibility;
using EnterpriseAddLogs.BackgroundJobs;
using EnterpriseAddLogs.Services;
using Plugin.Jobs;
using Android;

namespace EnterpriseAddLogs.Droid
{
    [Activity(Label = "EnterpriseAddLogs", Icon = "@mipmap/icon", Theme = "@style/MainTheme", 
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
    Manifest.Permission.AccessCoarseLocation,
    Manifest.Permission.AccessFineLocation
};

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Instance = this;

            CrossCurrentActivity.Current.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);

            //finger print initialization.
            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);

            CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Ioc.Container = new AndroidBootstrapper().Bootstrap();

            global::Xamarin.Essentials.Platform.Init(this, bundle);

            Plugin.Iconize.Iconize.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);

            UserDialogs.Init(() => this);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            Stormlion.PhotoBrowser.Droid.Platform.Init(this);

            CrossJobs.ResolveJob = JobBuilder.ResolveJob;

            Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication(new App());

            Ioc.Container.Resolve<IMessageBus>().Subscribe<ExitAppMessage>(message =>
            {
                Finish();
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }
    }
}

