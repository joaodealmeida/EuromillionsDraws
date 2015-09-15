using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using PCLCrypto;

using System.Threading.Tasks;

namespace Euromillions
{
	public class RegisterPage : ContentPage
	{
		public RegisterPage()
		{
			try {
			BindingContext = new Login();

			var layout = new StackLayout { Padding = 10 };

			var label = new Label
			{
				Text = "Please Register",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.White,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				XAlign = TextAlignment.Center, // Center the text in the blue box.
				YAlign = TextAlignment.Center, // Center the text in the blue box.
			};

			layout.Children.Add(label);

			var username = new Entry { Placeholder = "Username" };
			username.SetBinding(Entry.TextProperty, "Username");
			layout.Children.Add(username);

			var password = new Entry { Placeholder = "Password", IsPassword = true };
			password.SetBinding(Entry.TextProperty, "Password");
			layout.Children.Add(password);

			var email = new Entry { Placeholder = "Email", Keyboard = Keyboard.Email };
			email.SetBinding(Entry.TextProperty, "Email");
			layout.Children.Add(email);


			var button = new Button { Text = " Register", TextColor = Color.White };

			layout.Children.Add(button);

			Content = new ScrollView { Content = layout };
			button.Clicked += Register;
			} catch (Exception exc)
			{
				DisplayAlert("Error",exc.Message,"OK");
			}
		}

		public async void Register(object sender, EventArgs e)
		{
			

				WebService conn = new WebService();

				Login lg = (Login)BindingContext;

				Login toSend = new Login();
				toSend.Email = lg.Email;
				toSend.Username = lg.Username;
				
				//Encrypt password with and Update Login Object to send on webservice
				var salt = Crypto.CreateSalt(64);
				var encrypted = Crypto.EncryptToSend(lg.Password,salt);
				toSend.Password = encrypted;


				var response =  await conn.PerformRegister (toSend,salt);
				if(response != null)
				{
					var dataReceived = response[0];
					if(dataReceived.result.Equals("True")){
						await DisplayAlert("Successfull", "Registration Complete.","OK");
						await Navigation.PopAsync();
					}
					else{
						await DisplayAlert("Error", dataReceived.error,"OK");
					}
				}

			}
			
		}
	}


