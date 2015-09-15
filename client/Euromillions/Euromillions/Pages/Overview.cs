using System;
using Xamarin.Forms;

namespace Euromillions
{
	public class Overview : ContentPage
	{
		public Overview (){

			var logo = new Image { Aspect = Aspect.AspectFill };
			logo.Source = ImageSource.FromFile ("icon_.png");
			var label = new Label
			{
				Text = "Welcome to SDIS APP",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.White,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				XAlign = TextAlignment.Center, // Center the text in the blue box.
				YAlign = TextAlignment.Center, // Center the text in the blue box.
			};


			Content = new StackLayout {
				Spacing = 10,
				Children = { logo, label }
			};

		}



	}
}


