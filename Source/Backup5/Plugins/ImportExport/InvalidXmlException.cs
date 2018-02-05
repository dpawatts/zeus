namespace Zeus.Admin.Plugins.ImportExport
{
	public class InvalidXmlException : ZeusException
	{
		public InvalidXmlException(string message)
			: base(message)
		{
		}
	}
}