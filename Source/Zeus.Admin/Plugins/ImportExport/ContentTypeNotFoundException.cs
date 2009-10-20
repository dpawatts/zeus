namespace Zeus.Admin.Plugins.ImportExport
{
	public class ContentTypeNotFoundException : ZeusException
	{
		public ContentTypeNotFoundException(string message)
			: base(message)
		{
		}
	}
}