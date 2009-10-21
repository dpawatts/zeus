namespace Zeus.Web.Security
{
	public interface IAuthenticationContextInitializer
	{
		void Initialize(IAuthenticationContextService authenticationContextService);
	}
}