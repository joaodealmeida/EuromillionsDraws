using System;
using System.Collections.Generic;
using System.Text;
using PCLCrypto;
using Xamarin.Forms;

using System.Threading.Tasks;

namespace Euromillions
{
	public class LoginPage : ContentPage
	{
		ActivityIndicator load;
		public LoginPage()
		{
			BindingContext = new Login();

			var layout = new StackLayout { Padding = 10 };

			load = new ActivityIndicator {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				IsRunning = true,
				IsVisible = false,
				Color = Color.Blue
			};

			var loginImage = new Image ();
			loginImage.Source = ImageSource.FromFile("login_image.png");

			layout.Children.Add(loginImage);

			var username = new Entry { Placeholder = "Username" };

			username.SetBinding(Entry.TextProperty, "Username" );
			layout.Children.Add(username);

			var password = new Entry { Placeholder = "Password", IsPassword = true };

			password.SetBinding(Entry.TextProperty, "Password");
			layout.Children.Add(password);



			var button = new Button { Text = "Sign In" };
			var button2 = new Button { Text = "Register" };

			if (Device.OS == TargetPlatform.iOS) {
				button.TextColor = Color.Black;
				button2.TextColor = Color.Black;
			} else {
				button.TextColor = Color.White;
				button2.TextColor = Color.White;
			}


			layout.Children.Add(button);
			layout.Children.Add (button2);

			Content = new ScrollView { Content = layout };
			button.Clicked += Login;
			button2.Clicked += Register;
		}

		public async void Register(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegisterPage ());
		}

		public async void Login(object sender, EventArgs e)
		{
			try
			{
				WebService conn = new WebService();
				Login lg = (Login)BindingContext;

				Login toSend = new Login();
				toSend.Email = lg.Email;
				toSend.Username = lg.Username;

				var saltResponse = await conn.GetSalt(lg.Username);

				byte[] userSalt = null;
				if(saltResponse != null)
				{
					var saltData = saltResponse[0];


					if(saltData.salt != null){
						//Workaround - decoder for b64
						string r1 = saltData.salt.Replace('-','+');
						string r2 = r1.Replace('_','/');
						string finalB64 = r2.Replace(',','=');

						userSalt = System.Convert.FromBase64String(finalB64);
						
					}
					else{
						await DisplayAlert("Error", "This username does not exist","OK");
					}
				}
				else{
					await DisplayAlert("Error", "This username does not exist", "OK");
				}
				//Encrypt password with and Update Login Object to send on webservice

				toSend.Password = Crypto.EncryptToSend(lg.Password, userSalt);



				var response =  await conn.PerformLogin (toSend);

				if(response != null)
				{
					var dataReceived = response[0];


					if(dataReceived.result.Equals("True")){
						App.Current.Properties["key"] = dataReceived.key;

						var userID = await conn.GetID(dataReceived.key,toSend.Username);
						if(userID[0].id != null){
							App.Current.Properties["id"] = userID[0].id as string;
						}

						var pushToken =  conn.SendToken(Application.Current.Properties ["key"] as string,Application.Current.Properties ["token"] as string);

							

						App.Current.MainPage = new Homepage();
					}
					else{
						await DisplayAlert("Error", "Invalid Login/Password","OK");
					}
				}

			}
			catch (Exception exc)
			{
				DisplayAlert("Error",exc.Message,"OK");
			}
		}
	}
}