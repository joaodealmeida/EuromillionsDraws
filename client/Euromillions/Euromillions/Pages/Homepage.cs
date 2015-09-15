using System;
using Xamarin.Forms;

namespace Euromillions
{
	class Homepage : TabbedPage
	{
		public Homepage ()
		{
			this.Title = "Homepage";
			this.Children.Add(
				new NavigationPage( new Overview ())
				{
					Title = "Overview"
				});
			this.Children.Add(
				new NavigationPage( new MyBets ()) {
					Title = "Bets"
				});

			this.Children.Add(
				new NavigationPage( new AboutPAge()) {
					Title = "About"
				});
		}
	}
		
}	