using System;
using System.Collections.Generic;

namespace Zeus.FileSystem
{
	public class DatabaseFolder : IFolder
	{
		public int ID
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public IFolder ParentFolder
		{
			get;
			set;
		}

		public ICollection<IFolder> Folders
		{
			get;
			set;
		}

		public ICollection<IFile> Files
		{
			get;
			set;
		}
	}
}
