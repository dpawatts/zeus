using Zeus.Web;

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
			get { return Name; }
			set { Name = value; }
		}
	}
}
