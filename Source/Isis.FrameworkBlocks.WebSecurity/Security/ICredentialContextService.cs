namespace Isis.Web.Security
{
	public interface ICredentialContextService
	{
		ICredentialService GetCurrentService();
		void AddLocation(CredentialLocation location);
		bool ContainsLocation(string locationPath);
		void SetRootLocation(CredentialLocation rootLocation);
	}
}