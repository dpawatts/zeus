using System;

namespace Zeus.FileSystem
{
	public interface IFileSystem
	{
		IFolder RootFolder { get; }

		IFileIdentifier AddFile(string fileName, byte[] data);
		void DeleteFile(IFileIdentifier identifier);
		IFile GetFile(IFileIdentifier identifier);
	}
}
