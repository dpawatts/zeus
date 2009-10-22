using System;

namespace Zeus.BaseLibrary.IO
{
	public class ProgressChangedEventArgs : EventArgs
	{
		public long BytesRead;
		public long Length;
		public string FileName;

		public ProgressChangedEventArgs(long BytesRead, long Length, string fileName)
		{
			this.BytesRead = BytesRead;
			this.Length = Length;
			this.FileName = fileName;
		}
	}
}