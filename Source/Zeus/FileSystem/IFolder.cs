using System;
using System.Collections.Generic;

namespace Zeus.FileSystem
{
	public interface IFolder
	{
		string Name { get; }
		ICollection<IFolder> Folders { get; }
		ICollection<IFile> Files { get; }
	}
}
