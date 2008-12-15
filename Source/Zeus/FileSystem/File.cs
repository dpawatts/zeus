using System;

namespace Zeus.FileSystem
{
	public abstract class File : FileSystemNode
	{
		public IFileIdentifier Identifier
		{
			get { throw new NotImplementedException(); }
		}

		public string Name
		{
			get { throw new NotImplementedException(); }
		}

		public byte[] Data
		{
			get { throw new NotImplementedException(); }
		}
	}
}
