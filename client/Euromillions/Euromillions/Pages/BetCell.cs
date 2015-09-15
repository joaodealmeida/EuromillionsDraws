using System;
using Xamarin.Forms;

namespace Euromillions
{
	public class BetCell : ViewCell
	{
		Image n1, n2, n3, n4, n5, s1, s2, status,waiting,thumb_up,thumb_down;
		System.Collections.Generic.List<Image> numbersList;
		System.Collections.Generic.List<Image> starsList;

		public BetCell ()
		{
			numbersList = new System.Collections.Generic.List<Image> ();
			starsList = new System.Collections.Generic.List<Image> ();

			n1 = new Image { WidthRequest = 25, HeightRequest = 25 }; numbersList.Add (n1);
			n2 = new Image { WidthRequest = 25, HeightRequest = 25 }; numbersList.Add (n2);
			n3 = new Image { WidthRequest = 25, HeightRequest = 25 }; numbersList.Add (n3);
			n4 = new Image { WidthRequest = 25, HeightRequest = 25 }; numbersList.Add (n4);
			n5 = new Image { WidthRequest = 25, HeightRequest = 25 }; numbersList.Add (n5);
			s1 = new Image { WidthRequest = 25, HeightRequest = 25 }; starsList.Add (s1);
			s2 = new Image { WidthRequest = 25, HeightRequest = 25 }; starsList.Add (s2);
			status = new Image { HeightRequest = 25, HorizontalOptions = LayoutOptions.End };

			//Status Images
			waiting = new Image { HeightRequest = 25, HorizontalOptions = LayoutOptions.End };
			waiting.Source = ImageSource.FromFile ("waiting.png");
			thumb_down = new Image { HeightRequest = 25, HorizontalOptions = LayoutOptions.End };
			thumb_down.Source = ImageSource.FromFile ("thumb_down.png");

			thumb_up = new Image { HeightRequest = 25, HorizontalOptions = LayoutOptions.End };
			thumb_up.Source = ImageSource.FromFile ("thumb_up.png");


			var numbersLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 15,
				Children = {n1,n2,n3,n4,n5}
			};

			var starsLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 15,
				Children = { s1, s2 }
			};

			var detailsLayout = new StackLayout {
				Padding = new Thickness (10, 0, 0, 0),
				Spacing = 2,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = 
				{ 
					numbersLayout, starsLayout
				}
				};

			var cellLayout = new StackLayout {
				Spacing = 0,
				Padding = new Thickness (0, 5, 10, 5),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { detailsLayout, status }
			};


			View = cellLayout;
		}

		private void GetImagesFromBet(Bet betObj){
	
			string[] numbers = betObj.Numbers.Split ('-');
			string[] stars = betObj.Stars.Split ('-');
			int counter = 0;
			int counter2 = 0;


			//Load images of the numbers in the Bet
			foreach (string num in numbers) {
				numbersList[counter].Source = ImageSource.FromFile ("n"+num+".png");
				counter++;
			}

			//Load the images of the stars in the Bet
			foreach (string star in stars) {
				starsList[counter2].Source = ImageSource.FromFile ("star"+star+".png");
				counter2++;
			}

		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			GetImagesFromBet ((Bet)this.BindingContext);

			var bet = (Bet)this.BindingContext;

			if (bet.Status.Equals ("WAIT")) {
				status.Source = waiting.Source;
			} else if (bet.Status.Equals ("WIN")) {
				status.Source = thumb_up.Source;
			} else if (bet.Status.Equals ("LOSS")) {
				status.Source = thumb_down.Source;
			}

		}



	}
}

