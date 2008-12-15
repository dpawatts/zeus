using System;
using System.Collections.Generic;
using System.IO;
using Isis;
using System.Diagnostics;
using Zeus.Web;

namespace Zeus.FileSystem
{
	public class NtfsFolder : Folder
	{
		public NtfsFolder(Folder parent, DirectoryInfo directory)
		{
			this.Name = directory.Name;
			this.Title = directory.Name;
			this.Updated = directory.LastWriteTime;
			this.Created = directory.CreationTime;
			this.PhysicalPath = directory.FullName;
			this.Parent = this;
			((IUrlParserDependency) this).SetUrlParser(Zeus.Context.UrlParser);
		}

		protected override IList<Folder> GetDirectories()
		{
			try
			{
				DirectoryInfo currentDirectory = new DirectoryInfo(PhysicalPath);
				DirectoryInfo[] subDirectories = currentDirectory.GetDirectories();
				List<Folder> directories = new List<Folder>(subDirectories.Length);
				foreach (DirectoryInfo subDirectory in subDirectories)
					if ((subDirectory.Attributes & FileAttributes.Hidden) == 0)
						directories.Add(new NtfsFolder(this, subDirectory));
				return directories;
			}
			catch (DirectoryNotFoundException ex)
			{
				Trace.TraceWarning(ex.ToString());
				return new List<Folder>();
			}
		}

		protected override IList<File> GetFiles()
		{
			try
			{
				DirectoryInfo currentDirectory = new DirectoryInfo(PhysicalPath);
				FileInfo[] filesInDirectory = currentDirectory.GetFiles();
				List<File> files = new List<File>(filesInDirectory.Length);
				foreach (FileInfo fi in filesInDirectory)
					files.Add(new NtfsFile(this, fi));
				return files;
			}
			catch (DirectoryNotFoundException ex)
			{
				Trace.TraceWarning(ex.ToString());
				return new List<File>();
			}
		}
	}
}
