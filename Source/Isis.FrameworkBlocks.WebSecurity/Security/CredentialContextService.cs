using System;
using System.Linq;

namespace Isis.Web.Security
{
	public class CredentialContextService : ICredentialContextService
	{
		private readonly IWebContext _webContext;
		private CredentialLocation _rootLocation;

		public CredentialContextService(IWebContext webContext)
		{
			_webContext = webContext;
			_rootLocation = new CredentialLocation { Repository = new DefaultCredentialRepository() };
		}

		public void AddLocation(CredentialLocation location)
		{
			_rootLocation.ChildLocations.Add(location);
		}

		public bool ContainsLocation(string locationPath)
		{
			return _rootLocation.ChildLocations.Any(l => l.Path == locationPath);
		}

		public void SetRootLocation(CredentialLocation rootLocation)
		{
			_rootLocation = rootLocation;
		}

		public ICredentialService GetCurrentService()
		{
			CredentialLocation location = (CredentialLocation) _rootLocation.GetChild(_webContext.Url.Path);
			return new CredentialService(location.Repository);
		}
	}
}