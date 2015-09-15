using System;



namespace Euromillions
{
	public interface IImageResizer
	{
		byte[] ResizeImage (byte[] imageData, float width, float height);
	}
}