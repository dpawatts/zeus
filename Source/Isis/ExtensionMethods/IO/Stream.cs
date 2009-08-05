using System.IO;

namespace Isis.ExtensionMethods.IO
{
	public static class StreamExtensionMethods
	{
		public static byte[] ReadAllBytes(this Stream stream)
		{
			using (MemoryStream ms = new MemoryStream((int) stream.Length))
			{
				byte[] buffer = new byte[4096];
				int bytesRead;
				while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
				{
					ms.Write(buffer, 0, bytesRead);
				}
				return ms.ToArray();
			}
		}
	}
}