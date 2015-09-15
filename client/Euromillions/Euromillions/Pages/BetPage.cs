using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Euromillions
{
	public class TextToNumbersConverter : IValueConverter
	{
		public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var s = value as string;
			string[] helper;
			List<int> ret = new List<int> ();
			const char explode = '+';

			if (s == null) {
				throw new NotImplementedException ();
			}
			else {
				helper = s.Split (explode);
				foreach (string number in helper) {
					int val = -1;
					val = System.Convert.ToInt32 (number);
					ret.Add (val);
				}
				int[] numbersArray = ret.ToArray ();
				return numbersArray;
			}




		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}

	public class DateTimeToStringConverter : IValueConverter
	{
		public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var s = value as string;

			if (s == null) {
				DateTime myDate = DateTime.Now;
				return myDate;
			}
			else {
				DateTime myDate = DateTime.Parse (s, System.Globalization.CultureInfo.InvariantCulture);
				return myDate;
			}

		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var s = (DateTime)value;

			if (s == null) {
				throw new NotImplementedException ();
			}
			else {
				string ret = s.Date.ToString ();
				return ret;
			}
		}
	}

	public class BetPage : ContentPage
	{
		public BetPage ()
		{
			try{
			this.Title = "Test Title";

			NavigationPage.SetHasNavigationBar (this, true);
			var numbersLabel = new Label { Text = "Numbers" };
			var numbersEntry = new Entry ();
			
			numbersEntry.SetBinding(Entry.TextProperty, "Numbers");

			var starsLabel = new Label { Text = "Stars" };
			var starsEntry = new Entry ();
			
			starsEntry.SetBinding(Entry.TextProperty, "Stars");
			
			var dateLabel = new Label { Text = "Date" };
			var dateEntry = new DatePicker ();
			dateEntry.SetBinding (DatePicker.DateProperty, new Binding ("Date", BindingMode.TwoWay, new DateTimeToStringConverter (), null));
			

			var saveButton = new Button { Text = "Save" };
			saveButton.Clicked += (sender, e) => {
				var BetObject = (Bet)BindingContext;
				App.Database.SaveBet(BetObject);
				this.Navigation.PopAsync();

			};

			var deleteButton = new Button { Text = "Delete" };
			deleteButton.Clicked += (sender, e) => {
				var BetObject = (Bet)BindingContext;
				App.Database.DeleteBet(BetObject.ID);
				this.Navigation.PopAsync();

			};

			var cancelButton = new Button { Text = "Cancel" };
			cancelButton.Clicked += (sender, e) => {
				this.Navigation.PopAsync();
			};


			Content = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = new Thickness(20),
				Children = {
					numbersLabel, numbersEntry, 
					starsLabel, starsEntry,
					/*dateLabel, dateEntry,*/
					saveButton, deleteButton, cancelButton
				}
			};
			}
			catch(Exception exc){
				System.Diagnostics.Debug.WriteLine (exc);
			}
		}
	}
}
