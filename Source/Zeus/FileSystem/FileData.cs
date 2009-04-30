using System;

namespace Zeus.FileSystem
{
	[Serializable]
	public class FileData
	{
		public string FileName { get; set; }

		public string FileExtension
		{
			get { return System.IO.Path.GetExtension(FileName); }
		}

		public byte[] Data { get; set; }
		public string ContentType { get; set; }
		public long? Size { get; set; }
	}
}
