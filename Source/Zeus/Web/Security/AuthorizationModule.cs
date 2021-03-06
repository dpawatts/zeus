using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.Configuration;

namespace Zeus.Web.Security
{
	public class AuthorizationModule : IHttpModule
	{
		public void Init(HttpApplication app)
		{
			app.AuthorizeRequest += OnAuthorizeRequest;
		}

		private void OnAuthorizeRequest(object source, EventArgs eventArgs)
		{
			HttpApplication application = (HttpApplication) source;
			HttpContext context = application.Context;

			// If an <authorization> section exists in the current web.config, and it hasn't yet
			// been registered with the AuthorizationService, add it now.
			IAuthorizationService authorizationService = WebSecurityEngine.Get<IAuthorizationService>();
			AuthorizationSection authorizationSection = System.Web.Configuration.WebConfigurationManager.GetSection("zeus.web/authorization") as AuthorizationSection;
			string locationPath = context.Request.Path.ToLower();
			if (authorizationSection != null && !authorizationService.LocationExists(locationPath))
			{
				List<AuthorizationRule> rules = new List<AuthorizationRule>();
				foreach (AuthorizationRule authorizationRule in authorizationSection.Rules)
				{
					AuthorizationRule rule = new AuthorizationRule();
					rule.Action = (authorizationRule.Action == AuthorizationRuleAction.Allow) ? AuthorizationRuleAction.Allow : AuthorizationRuleAction.Deny;
					rule.Roles.AddRange(authorizationRule.Roles);
					rule.Users.AddRange(authorizationRule.Users);
					rules.Add(rule);
				}
				authorizationService.AddRules(locationPath, rules);
			}

			if (!context.SkipAuthorization)
			{
				if (!authorizationService.CheckUrlAccessForPrincipal(context.Request.Url.AbsolutePath, context.User, context.Request.RequestType))
				{
					context.Response.StatusCode = 0x191;
					application.CompleteRequest();
				}
			}
		}

		public void Dispose()
		{
		}
	}
}