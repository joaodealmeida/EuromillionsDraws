using System;
using Xamarin.Forms;

using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Euromillions
{
	public class CameraView : ContentPage
	{
		bool done;
		byte[] imgByte;
		//UI
		StackLayout layout;
		Button btn, btn1, btn2;

		public CameraView ()
		{
			done = false;
			this.layout = new StackLayout{ Padding = 10 };

			btn = new Button ();
			btn1 = new Button ();
			btn2 = new Button ();

			btn.Text = "Take picture";
			btn1.Text = " Select Picture";
			btn2.Text = "Add ticket";

			imgByte = null;
			_imageSource = new Image ();
			_imageSource.Aspect = Aspect.AspectFill; 

			layout.Children.Add (btn);
			layout.Children.Add (btn1);
			layout.Children.Add (_imageSource);

			btn.Clicked+= Btn_Clicked;
			btn1.Clicked+= Btn1_Clicked;
			btn2.Clicked+= Btn2_Clicked;



			Content = new ScrollView { Content = layout };
		}

		async void Btn2_Clicked (object sender, EventArgs e)
		{	try{
				
				WebService conn = new WebService ();
				string token = Application.Current.Properties ["key"] as string;


				var response =  await conn.RequestOCR (imgByte,token);


				if(response != null)
				{
					var dataReceived = response[0];	
					if(dataReceived.result.Equals("True")){
						await DisplayAlert("Message", "Your photo has been sucessfully sent","OK");
						bool _done = false;
						List<Dictionary<string, dynamic>> rep2 = null;
						await App.Sleep(2000);
						while(!_done){
							rep2 = await conn.GetTicket(token, dataReceived.request);
							var responseResult = rep2[0]["result"];
							if(!responseResult.Equals("WAIT"))
							{
								_done = true;
							}
							else
							{
								await App.Sleep(1000);
							}
						}
						var combinationResult = rep2[0]["response"];
						var msg  = JsonConvert.DeserializeObject<HelperAPI>(rep2[0]["response"]);
						await conn.DeleteTicket(token, dataReceived.request);
						if(rep2[0]["result"])
						{
							await DisplayAlert("Message", "Ticket Received","OK");
							//Move to AddPage
							var BetObject = new Bet();
							var BetPage = new BetPage();
							BetObject.Date = DateTime.Now.ToString ();
							//Parse string[] to string
							string numbersFinal = "";
							string starsFinal = "";
							int counter = 0;
							foreach (string n in msg.numbers)
							{
								if(counter != 4){
									numbersFinal +=n+"-";
								}
								else {numbersFinal +=n;
								}
							}
							counter = 0;
							foreach (string s in msg.stars)
							{
								if(counter != 1){
									starsFinal +=s+"-";
								}else{
								starsFinal +=s;
								}
							}
							BetObject.Numbers = numbersFinal;
							BetObject.Stars = starsFinal;
							BetPage.BindingContext = BetObject;
							Navigation.PushAsync(BetPage);


						}
						else{
							await DisplayAlert("Message", "Some error ocurred" ,"OK");
						}

					}
					else{
						await DisplayAlert("Error", "Deu merda amigo","OK");
					}
				}



			}
			catch(Exception exc){
				DisplayAlert("Error",exc.Message,"OK");
			}

		}

		async void Btn_Clicked (object sender, EventArgs e)
		{
			await TakePicture(_imageSource);
		}

		async void Btn1_Clicked (object sender, EventArgs e)
		{
			await SelectPicture(_imageSource);
		}

		 

		/// </summary>
		private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		/// <summary>
		/// The picture chooser.
		/// </summary>
		private IMediaPicker _mediaPicker;

		/// <summary>
		/// The image source.
		/// </summary>
		private Image _imageSource;

		/// Setups this instance.
		/// 
		private void InitMediaPicker()
		{
			if (_mediaPicker != null)
			{
				return;
			}

			var device = Resolver.Resolve<IDevice>();

			_mediaPicker = DependencyService.Get<IMediaPicker>();
			//RM: hack for working on windows phone? 
			if (_mediaPicker == null)
			{
				_mediaPicker = device.MediaPicker;
			}
		}
		private string _status;

		public static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream()){
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}

		private async Task<MediaFile> TakePicture(Image _imageSource)
		{
			InitMediaPicker();
			// string directory = _fileService.GetImageSavedRoute();

			return await this._mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions
				{
					DefaultCamera = CameraDevice.Front,
					MaxPixelDimension = 400
				}).ContinueWith(t =>
					{
						if (t.IsFaulted)
						{
							//this.Status = t.Exception.InnerException.ToString();
						}
						else if (t.IsCanceled)
						{
							//this.Status = "Canceled";
							return null;
						}
						else
						{
							var mediaFile = t.Result;
							this.layout.Children.Remove (btn);
							this.layout.Children.Remove (btn1);
							this.layout.Children.Add (btn2);


							imgByte = DependencyService.Get<IImageResizer>().ResizeImage(ReadFully(mediaFile.Source),1920,1080);

							_imageSource.Source = ImageSource.FromStream( () => new MemoryStream(imgByte));



							return mediaFile;
						}

						return null;
					}, _scheduler);
		}

		private async Task<MediaFile> SelectPicture (Image _imageSource)
		{
			return await this._mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
				{
					DefaultCamera = CameraDevice.Front,
					MaxPixelDimension = 400
				}).ContinueWith(t =>
					{
						if (t.IsFaulted)
						{
							//this.Status = t.Exception.InnerException.ToString();
						}
						else if (t.IsCanceled)
						{
							//this.Status = "Canceled";
							return null;
						}
						else
						{
							this.layout.Children.Remove (btn);
							this.layout.Children.Remove (btn1);
							this.layout.Children.Add (btn2);
							var mediaFile = t.Result;

							imgByte = DependencyService.Get<IImageResizer>().ResizeImage(ReadFully(mediaFile.Source),1920,1080);

							_imageSource.Source = ImageSource.FromStream( () => new MemoryStream(imgByte));



							return mediaFile;
						}

						return null;
					}, _scheduler);                  

		}
			
			
	}
}

