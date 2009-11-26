using System;
using System.Linq;
using System.Web.Configuration;
using AuthenticationSection=Zeus.Configuration.AuthenticationSection;

namespace Zeus.Web.Security
{
	public class AuthenticationContextService : IAuthenticationContextService
	{
		private readonly BaseLibrary.Web.IWebContext _webContext;
		private readonly AuthenticationLocation _rootLocation;

		public AuthenticationContextService(BaseLibrary.Web.IWebContext webContext, IAuthenticationContextInitializer[] authenticationContextInitializers)
		{
			_webContext = webContext;

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
			AuthenticationLocation location = (AuthenticationLocation) _rootLocation.GetChild(_webContext.Url.Path);
			string loginUrl = location.LoginUrl;
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
			return new AuthenticationService(_webContext, location, loginUrl);
		}
	}
}