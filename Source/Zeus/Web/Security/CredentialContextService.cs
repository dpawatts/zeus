using System.Linq;
using Ninject;

namespace Zeus.Web.Security
{
	public class CredentialContextService : ICredentialContextService
	{
		private readonly BaseLibrary.Web.IWebContext _webContext;
		private CredentialLocation _rootLocation;

		[Optional]
		public ICredentialContextInitializer[] CredentialContextInitializers { get; set; }

		public CredentialContextService(BaseLibrary.Web.IWebContext webContext)
		{
			_webContext = webContext;
			_rootLocation = new CredentialLocation { Repository = new DefaultCredentialRepository() };

			Initialize();
		}

		private void Initialize()
		{
			if (CredentialContextInitializers != null)
				foreach (ICredentialContextInitializer initializer in CredentialContextInitializers)
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