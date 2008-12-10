using System;

namespace Zeus.FileSystem
{
	[Serializable]
	public class DatabaseFileIdentifier : IFileIdentifier
	{
		public int FileID { get; set; }
	}
}
