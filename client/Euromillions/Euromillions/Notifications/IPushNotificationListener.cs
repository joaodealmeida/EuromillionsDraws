using System;
using System.Collections.Generic;
using System.Diagnostics;

using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using Xamarin.Forms;
using System.Linq;
using Newtonsoft.Json;

namespace Euromillions
{
	/// <summary>
	/// Device type.
	/// </summary>

	public class  CrossPushNotificationListener : IPushNotificationListener
	{
		//Here you will receive all push notification messages
		//Messages arrives as a dictionary, the device type is also sent in order to check specific keys correctly depending on the platform.
		public async void OnMessage(IDictionary<string, object> Parameters, DeviceType deviceType)
		{
			Debug.WriteLine("Message Arrived");
			//UPDATE ALL BETS FROM CLOUD
			WebService conn = new WebService ();
			var dbUpdated =  await conn.GetData(App.Current.Properties["key"] as string);
			DependencyService.Get<ISQLite>().OverrideDatabase(dbUpdated[0].data);
			Debug.WriteLine("Data UPDATED");

			IEnumerable<Bet> allBets = App.Database.GetBets ().ToList ();
			List<Dictionary<string, dynamic>> rep2 = null;
	
			rep2 = await conn.GetResults (App.Current.Properties ["key"] as string);
			var msg  = JsonConvert.DeserializeObject<HelperAPI>(rep2[0]["drawn"]);

			Bet Sorteio = new Bet ();

			//Parse string[] to string
			string numbersFinal = "";
			string starsFinal = "";
			int counter = 0;
			foreach (string n in msg.numbers)
			{
				if (counter != 4) {
					numbersFinal += n + "-";
				} else {
					numbersFinal += n;
				}
			}
			counter = 0;
			foreach (string s in msg.stars)
			{
				if (counter != 1) {
					starsFinal += s + "-";
				} else {
					starsFinal += s;
				}
			}
			Sorteio.Numbers = numbersFinal;
			Sorteio.Stars = starsFinal;
			Sorteio.Date = DateTime.Now.ToString ();



			BetResult BetRes = new BetResult ();
			foreach (Bet b in allBets)
			{
				//Consoante resultado da funcao checkPrize();
				if (b.Status.Equals("WAIT"))
					{
					//Consoante resultado da funcao checkPrize();
					if (BetRes.checkPrize (b, Sorteio)) {
						b.Status = "WIN";
					} else {
						b.Status = "LOSS";
					}

					App.Database.SaveBet (b);
					}
			}


		}
		//Gets the registration token after push registration
		public void OnRegistered(string Token, DeviceType deviceType)
		{
			Application.Current.Properties ["token"] = Token;

			Debug.WriteLine (string.Format ("Push Notification - Device Registered - Token : {0}", Token));
		}

		//Fires when device is unregistered
		public void OnUnregistered(DeviceType deviceType)
		{
			Debug.WriteLine("Push Notification - Device Unnregistered");

		}

		//Fires when error
		public void OnError(string message, DeviceType deviceType)
		{
			Debug.WriteLine(string.Format("Push notification error - {0}",message));
		}
	}
}

