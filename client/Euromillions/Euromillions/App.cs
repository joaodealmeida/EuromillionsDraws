using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;


using Xamarin.Forms;
using PushNotification.Plugin;
namespace Euromillions
{
	public class App : Application
	{
		static BetDatabase database;

		public static BetDatabase Database
		{
			get
			{
				if (database == null){
					database = new BetDatabase ();
				}

				return database;
			}
		}

		public static async Task Sleep(int ms){
			await Task.Delay(ms);
		}

		public int ResumeAtBetId { get; set;}

		public App()
		{
			// The root page of your application
			if (App.Current.Properties.ContainsKey ("key")) {
				var key = Application.Current.Properties ["key"] as string;
				if (key != "null") {
					MainPage = new Homepage ();
				} else {
					MainPage = new NavigationPage (new LoginPage ());
				}
			} else {
				MainPage = new NavigationPage (new LoginPage ());
			}

		}

		protected override void OnStart()
		{

			CrossPushNotification.Current.Register();
			// Handle when your app starts
			if (Properties.ContainsKey ("ResumeAtBetId")) {
				var rati = Properties ["ResumeAtBetId"].ToString ();
				if (!String.IsNullOrEmpty (rati)) {
					ResumeAtBetId = int.Parse (rati);
				}
			}


		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			Properties ["ResumeAtBetId"] = ResumeAtBetId;

		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
