using System;
using System.Collections.Generic;
using Zeus.Persistence;
using System.Configuration;
using Zeus.Configuration;
using NHibernate;

namespace Zeus.FileSystem
{
	public class DatabaseFileSystem : IFileSystem
	{
		private ISession CurrentSession
		{
			get { return Zeus.Context.Current.Resolve<ISessionProvider>().OpenSession; }
		}

		public IFolder RootFolder
		{
			get
			{
				DatabaseFolder rootFolder = this.CurrentSession.Get<DatabaseFolder>(Convert.ToInt32(((FileSystemSection) ConfigurationManager.GetSection("zeus/fileSystem")).Settings["RootFolderID"].Value));
				return rootFolder;
			}
		}

		public IFileIdentifier AddFile(string fileName, byte[] data)
		{
			DatabaseFile file = new DatabaseFile { Folder = this.RootFolder, Name = fileName, Data = data };
			this.CurrentSession.Save(file);
			return ((IFile) file).Identifier;
		}

		public void DeleteFile(IFileIdentifier identifier)
		{
			DatabaseFileIdentifier fileIdentifier = (DatabaseFileIdentifier) identifier;
			this.CurrentSession.Delete(this.CurrentSession.Get<DatabaseFile>(fileIdentifier.FileID));
		}

		public IFile GetFile(IFileIdentifier identifier)
		{
			DatabaseFileIdentifier fileIdentifier = (DatabaseFileIdentifier) identifier;
			return this.CurrentSession.Get<DatabaseFile>(fileIdentifier.FileID);
		}
	}
}
