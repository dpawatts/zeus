namespace Zeus.Web.Security
{
	public class CredentialLocation : SecurityLocation
	{
		public ICredentialRepository Repository { get; set; }
	}
}