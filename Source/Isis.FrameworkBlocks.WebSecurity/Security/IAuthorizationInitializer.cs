namespace Isis.Web.Security
{
	public interface IAuthorizationInitializer
	{
		void Initialize(IAuthorizationService authorizationService);
	}
}