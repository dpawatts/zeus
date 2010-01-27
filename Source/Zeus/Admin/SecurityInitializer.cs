using System;
using System.Collections.Generic;
using System.Configuration;
using Zeus.Configuration;
using Zeus.Web.Security;

namespace Zeus.Admin
{
	public class SecurityInitializer : IAuthorizationInitializer, IAuthenticationContextInitializer
	{
		private readonly AdminSection _adminConfig;

		public SecurityInitializer(AdminSection adminConfig)
		{
			_adminConfig = adminConfig;
		}

		public void Initialize(IAuthorizationService authorizationService)
		{
			// If site is currently in Install mode, don't do anything.
			if (_adminConfig.Installer.Mode == InstallationMode.Install)
				return;

			// Dynamically add authorization rule for admin site.
			List<string> authorizedRoles = new List<string>();
			foreach (AuthorizedRoleElement authorizedRoleElement in _adminConfig.AuthorizedRoles)
				authorizedRoles.Add(authorizedRoleElement.Role);
			authorizationService.AddRule(_adminConfig.Path,
				null, authorizedRoles, new[] { "*" }, null);
		}

		public void Initialize(IAuthenticationContextService authenticationContextService)
		{
			authenticationContextService.AddLocation(new AuthenticationLocation
			{
				Enabled = true,
				Path = _adminConfig.Path,
				Name = ".ISISWEBAUTH.ADMIN",
				LoginUrl = "~/" + _adminConfig.Path + "/login.aspx",
				DefaultUrl = "~/" + _adminConfig.Path + "/default.aspx",
				Timeout = TimeSpan.FromMinutes(60),
				CookiePath = "/",
				CookieDomain = string.Empty
			});
		}
	}
}