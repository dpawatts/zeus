using System.IO;

namespace Zeus.BaseLibrary.ExtensionMethods.IO
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

		public static void CopyTo(this Stream input, Stream output)
		{
			const int size = 4096;
			byte[] bytes = new byte[4096];
			int numBytes;
			while ((numBytes = input.Read(bytes, 0, size)) > 0)
				output.Write(bytes, 0, numBytes);
		}
	}
}