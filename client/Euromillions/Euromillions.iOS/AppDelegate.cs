using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using PushNotification.Plugin;

using System.IO;

namespace Euromillions.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			#region Resolver Init
			SimpleContainer container = new SimpleContainer();
			container.Register<IDevice>(t => AppleDevice.CurrentDevice);
			container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
			container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

			Resolver.SetResolver(container.GetResolver());
			#endregion

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
			CrossPushNotification.Initialize<CrossPushNotificationListener>();

            return base.FinishedLaunching(app, options);
        }

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnErrorReceived(error);
			}
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(deviceToken);
			}
		}

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		/*public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action completionHandler)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
		}*/

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{ 
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
		}
    }
}
