using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Zeus.Configuration;
using AuthenticationSection=Zeus.Configuration.AuthenticationSection;

namespace Zeus.Web.Security
{
	public class AuthenticationContextService : IAuthenticationContextService
	{
		private readonly BaseLibrary.Web.IWebContext _webContext;
		private readonly AdminSection _adminConfig;
		private readonly AuthenticationLocation _rootLocation;

		public AuthenticationContextService(BaseLibrary.Web.IWebContext webContext,
			IAuthenticationContextInitializer[] authenticationContextInitializers,
			AdminSection adminConfig)
		{
			_webContext = webContext;
			_adminConfig = adminConfig;

			AuthenticationSection authSection = WebConfigurationManager.GetSection("zeus/authentication") as AuthenticationSection;
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
			// If the current HTTP request is for a Zeus page, then check if that page or any of its ancestors
			// implement the ILoginContext interface. If so, use the LoginUrl from that page.
			// BUT only if we're not currently in installation mode.
			AuthenticationLocation location = (AuthenticationLocation) _rootLocation.GetChild(
				VirtualPathUtility.ToAppRelative(_webContext.Url.Path).TrimStart('~'));
			string loginUrl = location.LoginUrl;
			// If site is currently in Install mode, don't do anything.
			if (_adminConfig.Installer.Mode == InstallationMode.Normal
				&& !VirtualPathUtility.ToAppRelative(_webContext.Url.Path).ToLower().StartsWith("~/" + _adminConfig.Path.ToLower()))
			{
				ContentItem currentPage = Context.CurrentPage;
				while (currentPage != null)
				{
					if (currentPage is ILoginContext)
					{
						loginUrl = ((ILoginContext) currentPage).LoginUrl;
						break;
					}
					currentPage = currentPage.GetParent();
				}
			}
			return new AuthenticationService(_webContext, location, loginUrl);
		}
	}
}