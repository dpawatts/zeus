namespace Zeus.Web.Security
{
	public interface ICredentialContextInitializer
	{
		void Initialize(ICredentialContextService credentialContextService);
	}
}