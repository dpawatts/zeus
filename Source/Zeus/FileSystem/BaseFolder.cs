using System;
using System.Collections.Generic;
using Zeus.Linq.Filters;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Zeus.ContentTypes.Properties;

namespace Zeus.FileSystem
{
	public abstract class BaseFolder : FileSystemNode
	{
		public override string IconUrl
		{
			get { return "~/Admin/Assets/Images/Icons/folder.png"; }
		}

		[TextBoxEditor("Folder", 10)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		protected IList<ContentItem> GetDirectories()
		{
			try
			{
				DirectoryInfo currentDirectory = new DirectoryInfo(PhysicalPath);
				DirectoryInfo[] subDirectories = currentDirectory.GetDirectories();
				List<ContentItem> directories = new List<ContentItem>(subDirectories.Length);
				foreach (DirectoryInfo subDirectory in subDirectories)
					if ((subDirectory.Attributes & FileAttributes.Hidden) == 0)
						directories.Add(new Folder(this, subDirectory));
				return directories;
			}
			catch (DirectoryNotFoundException ex)
			{
				Trace.TraceWarning(ex.ToString());
				return new List<ContentItem>();
			}
		}

		protected IList<ContentItem> GetFiles()
		{
			try
			{
				DirectoryInfo currentDirectory = new DirectoryInfo(PhysicalPath);
				FileInfo[] filesInDirectory = currentDirectory.GetFiles();
				List<ContentItem> files = new List<ContentItem>(filesInDirectory.Length);
				foreach (FileInfo fi in filesInDirectory)
					files.Add(new File(this, fi));
				return files;
			}
			catch (DirectoryNotFoundException ex)
			{
				Trace.TraceWarning(ex.ToString());
				return new List<ContentItem>();
			}
		}

		public override IList<ContentItem> GetChildren(ItemFilter filter)
		{
			List<ContentItem> items = new List<ContentItem>();
			items.AddRange(filter.Filter(GetDirectories().AsQueryable()));
			items.AddRange(filter.Filter(GetFiles().AsQueryable()));
			return items;
		}

		public static BaseFolder EnsureFolder(ContentItem item)
		{
			if (item is BaseFolder)
				return item as BaseFolder;
			else
				throw new ZeusException(item + " is not a Folder.");
		}
	}
}
