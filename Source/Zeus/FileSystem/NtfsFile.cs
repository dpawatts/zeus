using System;
using System.IO;
using Zeus.Web;

namespace Zeus.FileSystem
{
	public class NtfsFile : File
	{
		public NtfsFile(Folder parent, FileInfo file)
		{
			this.Name = file.Name;
			this.Title = file.Name;
			this.Size = file.Length;
			this.Updated = file.LastWriteTime;
			this.Created = file.CreationTime;
			this.PhysicalPath = file.FullName;
			this.Parent = parent;
			((IUrlParserDependency) this).SetUrlParser(Zeus.Context.UrlParser);
		}
	}
}
