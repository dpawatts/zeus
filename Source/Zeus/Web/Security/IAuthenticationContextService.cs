namespace Zeus.Web.Security
{
	public interface IAuthenticationContextService
	{
		IAuthenticationService GetCurrentService();
		void AddLocation(AuthenticationLocation location);
		bool ContainsLocation(string locationPath);
	}
}