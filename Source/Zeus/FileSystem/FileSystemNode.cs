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
	}
}
