using System;
using System.Drawing.Imaging;

namespace Isis.Drawing.Imaging
{
	public static class ImageCodecInfoHelper
	{
		public static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
				if (codec.FormatID == format.Guid)
					return codec;
			return null;
		}
	}
}
