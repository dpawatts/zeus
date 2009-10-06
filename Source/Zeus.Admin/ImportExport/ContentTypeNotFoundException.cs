namespace Zeus.Admin.ImportExport
{
	public class ContentTypeNotFoundException : ZeusException
	{
		public ContentTypeNotFoundException(string message)
			: base(message)
		{
		}
	}
}