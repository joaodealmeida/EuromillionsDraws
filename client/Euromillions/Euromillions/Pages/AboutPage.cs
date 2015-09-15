using System;
using Xamarin.Forms;

namespace Euromillions
{
	public class AboutPAge : ContentPage
	{
		public AboutPAge (){


			var label = new Label
			{
				Text = "This application was made by Eduardo Almeida, João Almeida, Miguel Fernandes e Pedro Santiago para a UC SDIS 2014/2015",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.White,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				XAlign = TextAlignment.Center, // Center the text in the blue box.
				YAlign = TextAlignment.Center, // Center the text in the blue box.
			};


			Content = new StackLayout {
				Spacing = 10,
				Children = { label }
			};

		}



	}
}


