namespace Isis.Web.Security
{
	public class CredentialLocation : SecurityLocation
	{
		public ICredentialRepository Repository { get; set; }
	}
}