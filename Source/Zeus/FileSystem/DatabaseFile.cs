using System;
using System.Collections.Generic;

namespace Zeus.FileSystem
{
	public class DatabaseFile : File
	{
		public int ID
		{
			get;
			set;
		}

		IFileIdentifier IFile.Identifier
		{
			get { return new DatabaseFileIdentifier { FileID = this.ID }; }
		}

		public string Name
		{
			get;
			set;
		}

		public IFolder Folder
		{
			get;
			set;
		}

		public byte[] Data
		{
			get;
			set;
		}
	}
}
