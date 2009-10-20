using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Isis.Drawing
{
	public static class ImageHelper
	{
		/// <summary>
		/// You MUST call Dispose on the returned image, otherwise the source memory stream will be left open
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static Image FromBytes(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return null;

			MemoryStream stream = null;
			try
			{
				stream = new MemoryStream(bytes);
				Image result = (Image) Image.FromStream(stream).Clone();
				return result;
			}
			catch
			{
				if (stream != null)
					stream.Close();

				throw;
			}
		}

		public static byte[] ToBytes(Image image, ImageFormat format)
		{
			if (image == null)
				return null;

			MemoryStream stream = null;
			try
			{
				stream = new MemoryStream();
				image.Save(stream, format);
				byte[] result = stream.ToArray();
				return result;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}
		}
	}
}
