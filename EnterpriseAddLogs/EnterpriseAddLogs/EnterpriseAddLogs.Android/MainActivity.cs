using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Fingerprint;
using Plugin.SpeechRecognition;
using EnterpriseAddLogs.Messaging;
using Autofac;

namespace EnterpriseAddLogs.Droid
{
    [Activity(Label = "EnterpriseAddLogs", Icon = "@mipmap/icon", Theme = "@style/MainTheme", 
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

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

            global::Xamarin.Forms.Forms.Init(this, bundle);

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
    }
}

