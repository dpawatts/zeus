namespace Zeus.FileSystem
{
	public abstract class FileSystemNode : ContentItem
	{
		public override string Extension
		{
			get { return string.Empty; }
		}
	}
}
