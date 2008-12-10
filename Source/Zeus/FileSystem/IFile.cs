using System;

namespace Zeus.FileSystem
{
	public interface IFile
	{
		IFileIdentifier Identifier { get; }
		string Name { get; }
		byte[] Data { get; }
	}
}
