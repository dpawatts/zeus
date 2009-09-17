using System;
using Zeus.ContentTypes;

namespace Zeus.FileSystem
{
	public class FileSystemService : IFileSystemService
	{
		#region Fields

		private readonly IContentTypeManager _contentTypeManager;

		#endregion

		#region Constructor

		public FileSystemService(IContentTypeManager contentTypeManager)
		{
			_contentTypeManager = contentTypeManager;
		}

		#endregion

		public Folder EnsureFolder(Folder parentFolder, string folderName)
		{
			Folder currentFolder = parentFolder;

			string[] folderNameParts = folderName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string folderNamePart in folderNameParts)
			{
				Folder existingFolder = currentFolder.GetChild(folderNamePart) as Folder;
				if (existingFolder != null)
				{
					currentFolder = existingFolder;
					continue;
				}

				Folder newFolder = _contentTypeManager.CreateInstance<Folder>(currentFolder);
				newFolder.Name = folderNamePart;
				newFolder.AddTo(currentFolder);

				currentFolder = newFolder;
			}

			return currentFolder;
		}
	}
}