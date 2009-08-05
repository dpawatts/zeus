using System;
using System.Linq;
using System.Web.Configuration;
using Isis.Reflection;
using AuthenticationSection=Isis.Web.Configuration.AuthenticationSection;

namespace Isis.Web.Security
{
	public class AuthenticationContextService : IAuthenticationContextService
	{
		private readonly IWebContext _webContext;
		private readonly AuthenticationLocation _rootLocation;

		public AuthenticationContextService(IWebContext webContext, IAuthenticationContextInitializer[] authenticationContextInitializers)
		{
			_webContext = webContext;

			AuthenticationSection authSection = WebConfigurationManager.GetSection("isis.web/authentication", _webContext.Url.Path) as AuthenticationSection;
			if (authSection == null)
				authSection = new AuthenticationSection();
			_rootLocation = authSection.ToAuthenticationLocation();

			foreach (IAuthenticationContextInitializer initializer in authenticationContextInitializers)
				initializer.Initialize(this);
		}

		public void AddLocation(AuthenticationLocation location)
		{
			_rootLocation.ChildLocations.Add(location);
		}

		public bool ContainsLocation(string locationPath)
		{
			return _rootLocation.ChildLocations.Any(l => l.Path == locationPath);
		}

		public IAuthenticationService GetCurrentService()
		{
			AuthenticationLocation location = (AuthenticationLocation) _rootLocation.GetChild(_webContext.Url.Path);
			return new AuthenticationService(_webContext, location);
		}
	}
}