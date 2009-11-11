using System;
using System.Linq;

namespace Zeus.Web.Security
{
	public class CredentialContextService : ICredentialContextService
	{
		private readonly BaseLibrary.Web.IWebContext _webContext;
		private readonly ICredentialContextInitializer[] _credentialContextInitializers;
		private CredentialLocation _rootLocation;

		public CredentialContextService(BaseLibrary.Web.IWebContext webContext, ICredentialContextInitializer[] credentialContextInitializers)
		{
			_webContext = webContext;
			_credentialContextInitializers = credentialContextInitializers;
			_rootLocation = new CredentialLocation { Repository = new DefaultCredentialRepository() };

			Initialize();
		}

		private void Initialize()
		{
			foreach (ICredentialContextInitializer initializer in _credentialContextInitializers)
				initializer.Initialize(this);
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
			Initialize();
		}

		public ICredentialService GetCurrentService()
		{
			CredentialLocation location = (CredentialLocation) _rootLocation.GetChild(_webContext.Url.Path);
			return new CredentialService(location.Repository);
		}
	}
}