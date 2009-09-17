namespace Zeus.FileSystem
{
	public interface IFileSystemService
	{
		Folder EnsureFolder(Folder parentFolder, string folderName);
	}
}