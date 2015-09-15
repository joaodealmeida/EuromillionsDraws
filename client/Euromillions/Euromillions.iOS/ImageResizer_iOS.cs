using System;
using Xamarin.Forms;
using Euromillions.iOS;
using System.IO;

using System.Drawing;
using UIKit;
using CoreGraphics;

[assembly: Dependency (typeof (ImageResizer_iOS))]

namespace Euromillions.iOS
{
	public class ImageResizer_iOS : IImageResizer
	{

		public ImageResizer_iOS()
		{
			
		}

		public byte[] ResizeImage(byte[] imageData, float width, float height) {
			// Load the bitmap
			UIImage originalImage = ImageFromByteArray(imageData);
			//
			var Hoehe = originalImage.Size.Height;
			var Breite = originalImage.Size.Width;
			//
			nfloat ZielHoehe = 0;
			nfloat ZielBreite = 0;
			//

			if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
			{
				ZielHoehe = height;
				nfloat teiler = Hoehe / height;
				ZielBreite = Breite / teiler;
			}
			else // Breite (61 for Avatar) ist Master
			{
				ZielBreite = width;
				nfloat teiler = Breite / width;
				ZielHoehe = Hoehe / teiler;
			}
			//
			width = (float)ZielBreite;
			height = (float)ZielHoehe;
			//
			UIGraphics.BeginImageContext(new SizeF(width, height));
			originalImage.Draw(new RectangleF(0, 0, width, height));
			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			//
			var bytesImagen = resizedImage.AsJPEG().ToArray();
			resizedImage.Dispose();
			return bytesImagen;
		}

		public static UIKit.UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}
			//
			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}
	}
}