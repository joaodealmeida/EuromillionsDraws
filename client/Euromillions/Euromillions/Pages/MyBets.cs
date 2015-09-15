using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Euromillions
{
	public class NumbersConverter : IValueConverter
	{
		public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var s = value as int[];
			if (s == null)
				return "Error";

			return string.Format("Numbers: {0} + {1} + {2} + {3} + {4}",s[0],s[1],s[2],s[3],s[4]);
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}

	public class StarsConverter : IValueConverter
	{
		public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var s = value as int[];
			if (s == null)
				return "Error";

			return string.Format("Stars: {0} + {1}",s[0],s[1]);
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}

	class MyBets : ContentPage
	{
		ListView listView;

		public MyBets()
		{

			// Create the ListView.
			listView = new ListView();
			// Define template for displaying each item.
			listView.ItemTemplate = new DataTemplate(typeof(BetCell));
			//Define action for item selected
			listView.ItemSelected += (sender, e) => {
				var BetObject = (Bet)e.SelectedItem;
				var BetPage = new BetPage ();
				BetPage.BindingContext = BetObject;
				((App)App.Current).ResumeAtBetId = BetObject.ID;
				Navigation.PushAsync (BetPage);
			};

			// Accomodate iPhone status bar.
			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
			listView.HasUnevenRows = true;

			var tbi =  new ToolbarItem("+", null, async () =>
				{
					var action =  await DisplayActionSheet ("Add Bet", "Cancel", null, "Manually", "Picture");
					switch (action){
					case "Manually":
						{
							var BetObject = new Bet();
							var BetPage = new BetPage();
							BetPage.BindingContext = BetObject;
							Navigation.PushAsync(BetPage);
							break;
						}
					case "Picture":
						{
							Navigation.PushAsync( new CameraView());
							break;
						}
					}

				}
			);
			this.ToolbarItems.Add(tbi);

			listView.IsPullToRefreshEnabled = true;

			listView.Refreshing += ListView_Refreshing;

			// Build the page.
			this.Content = new StackLayout
			{
				Children = 
				{
					listView
				}
				};
		}

		public  async void ListView_Refreshing (object sender, EventArgs e)
		{
			WebService conn = new WebService ();
			var dbUpdated =  await conn.GetData(App.Current.Properties["key"] as string);
			DependencyService.Get<ISQLite>().OverrideDatabase(dbUpdated[0].data);
			listView.ItemsSource = App.Database.GetBets ();
			listView.EndRefresh ();
		}


		protected override void OnAppearing()
		{

			base.OnAppearing ();
			// reset the 'resume' id, since we just want to re-start here
			((App)App.Current).ResumeAtBetId = -1;
			listView.ItemsSource = App.Database.GetBets ();

		}
			
	}
}
