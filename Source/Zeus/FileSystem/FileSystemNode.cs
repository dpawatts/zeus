using System;

namespace Zeus.FileSystem
{
	public abstract class FileSystemNode : ContentItem
	{
		public override string Extension
		{
			get { return string.Empty; }
		}

		public override string Title
		{
			get { return this.Name; }
			set { this.Name = value; }
		}

		public virtual string PhysicalPath
		{
			get;
			set;
		}

		public virtual BaseFolder Folder
		{
			get { return this.Parent as BaseFolder; }
		}
	}
}
