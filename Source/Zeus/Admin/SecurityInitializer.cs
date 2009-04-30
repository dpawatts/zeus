using System;
using System.Configuration;
using Isis.ComponentModel;
using Isis.Web.Security;
using Zeus.Configuration;

namespace Zeus.Admin
{
	public class SecurityInitializer : IAuthorizationInitializer, IAuthenticationContextInitializer
	{
		public void Initialize(IAuthorizationService authorizationService)
		{
			// If site is currently in Install mode, don't do anything.
			AdminSection adminSection = ConfigurationManager.GetSection("zeus/admin") as AdminSection;
			if (adminSection != null && adminSection.Installer.Mode == InstallationMode.Install)
				return;

			// Dynamically add authorization rule for admin site.
			authorizationService.AddRule(IoC.Resolve<AdminSection>().Path,
				null, new[] { "Administrators", "Editors", "OrganisationEditors" },
				new[] { "*" }, null);
		}

		public void Initialize(IAuthenticationContextService authenticationContextService)
		{
			authenticationContextService.AddLocation(new AuthenticationLocation
			{
				Enabled = true,
				Path = IoC.Resolve<AdminSection>().Path,
				Name = ".ISISWEBAUTH.ADMIN",
				LoginUrl = "~/admin/login.aspx",
				DefaultUrl = "~/admin/default.aspx",
				Timeout = TimeSpan.FromMinutes(60),
				CookiePath = "/",
				CookieDomain = string.Empty
			});
		}
	}
}