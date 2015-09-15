//**************************************************************************
// Copyright 2015 - Shantimohan Elchuri (ESM Smart Solutions)
//   
//**************************************************************************
//  This code shows the usage of XLabs NuGet package's MediaPicker control.
//  This code is adopted from the original sample found in XLabs Wiki page.
//
//  The changes are:
//    In XAML:
//      1. Each control is given a name by setting x:Name attribute.
//      2. The bindings are removed to facilitate independent use of the view model.
//      3. The Command settings are replace by 'Clicked' events.
//
//    In Code:
//      1. The CameraViewModel is initialized in the page constructor.
//      2. 3 events are added to cater to the 3 buttons.
//      3. For all 3 platforms, the Resolver need to be initialized. See the code
//           in the #region named 'Resolver Init' in the following files.
//           a) XPA_PickMedia_XLabs_XFP.Droid.MainActivity.cs
//           b) XPA_PickMedia_XLabs_XFP.iOS.AppDelegate.cs
//           c) XPA_PickMedia_XLabs_XFP.WP80SL.MainPage.xaml.cs
//
//  How to adapt it into your own project:
//    1. Add 'CameraViewModel.cs' to the Shared / PCL project. I add as link to such utilities
//         so that changes to code are automatically reflected in the linked projects.
//    2. Add 'usgin XPA_PickMedia_XLabs_XPA' so that the methods in the view model are directly addressable.
//    3. Copy the required code from this:
//         a) The view model variable declaration
//         b) The view model initialization
//         c) The required events
//    4. Capabilites settings (check either or all of the follwoing in project options):
//         a) iOS: Not yet tested
//         b) Android (Project Options -> Build -> Android Application -> Required Permissions):
//               i) Camera (android.permissions.CAMERA)
//              ii) WriteExternalStorage (android.permissions.WRITE_EXTERNAL_STORATE)
//         c) Windows Phone (Project Properties (Solution Explorer) -> WMAppManifest.xml (File) -> Capabilities (Tab):
//               i) ID_CAP_ISV_CAMERA
//              ii) ID_CAP_MEDIALIB_PHOTO
//
//  Dated: Feb. 26, 2015
//    1. The NuGet packages used are:
//         a) Xamarin.Forms V1.4.0.6341
//         b) XLabs.Forms V2.0.5546.35667 (Installing this installs all dependent packages)
//    2. The code is tested in Android KitKat (GN4) & Windows Phone 8.1 (LUMIA 830) devices only.
//    3. The known issues are;
//         a) Not defaulting to the front camera, on both Android & WP.
//         b) On Android, picture taken in portrait is rotated 90 degrees anti-clockwise.
//         c) Lifecycle events are not coded, so crashes can occur.
//
//**************************************************************************
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Euromillions
{
	public partial class CameraPage : ContentPage
	{
		CameraViewModel cameraOps = null;
		Image imgPicked;
		Entry entDetails;


		public CameraPage ()
		{

			var btnTakePicture = new Button {
				Text = "Take Picture"
			};

			var btnPickPicture = new Button {
				Text = "Pick Picture"
			};

			var btnPickVideo = new Button {
				Text = "Pick Video"
			};


			imgPicked= new Image ();
			imgPicked.Source = null;
			imgPicked.VerticalOptions = LayoutOptions.CenterAndExpand;

			entDetails = new Entry ();
			entDetails.Text = null;
			entDetails.VerticalOptions = LayoutOptions.CenterAndExpand;


			btnTakePicture.Clicked += btnTakePicture_Clicked;
			btnPickPicture.Clicked += btnPickPicture_Clicked;
			btnPickVideo.Clicked += BtnPickVideo_Clicked;


     
			Content = new StackLayout {
				Children = { btnTakePicture, btnPickPicture, btnPickVideo }
			};

			cameraOps = new CameraViewModel();
		}

		void BtnPickVideo_Clicked (object sender, EventArgs e)
		{
			
		}

		private async void btnTakePicture_Clicked (object sender, EventArgs e)
		{
			await cameraOps.TakePicture ();
			imgPicked.Source = cameraOps.ImageSource;
			entDetails.Text = "";
		}

		private async void btnPickPicture_Clicked (object sender, EventArgs e)
		{
			await cameraOps.SelectPicture ();
			imgPicked.Source = cameraOps.ImageSource;
			entDetails.Text = "";
		}

		private async void btnPickVideo_Clicked (object sender, EventArgs e)
		{
			await cameraOps.SelectVideo ();
			imgPicked.Source = null;
			entDetails.Text = cameraOps.VideoInfo;
		}
	}
}


