using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using PushNotification.Plugin;

namespace Euromillions.Droid
{
    [Activity(Label = "Euromillions", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			//Needed for Camera
			#region Resolver Init
			SimpleContainer container = new SimpleContainer();
			container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
			container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
			container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

			Resolver.SetResolver(container.GetResolver());
			#endregion

            global::Xamarin.Forms.Forms.Init(this, bundle);
			CrossPushNotification.Initialize<CrossPushNotificationListener>("154440120942");

            LoadApplication(new App());
        }
    }
}

