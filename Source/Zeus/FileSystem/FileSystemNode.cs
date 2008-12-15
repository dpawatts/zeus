using System;

namespace Zeus.FileSystem
{
	public abstract class FileSystemNode : ContentItem
	{
		public string PhysicalPath
		{
			get;
			set;
		}
	}
}
