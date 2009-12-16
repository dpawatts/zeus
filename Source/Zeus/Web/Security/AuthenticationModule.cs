using System;
using System.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using System.Web.Security;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public class AuthenticationModule : IHttpModule
	{
		#region Fields

		private bool _onEnterCalled;

		#endregion

		#region Events

		public event EventHandler<AuthenticationEventArgs> Authenticate;

		#endregion

		#region Properties

		protected static IAuthenticationService CurrentAuthenticationService
		{
			get { return WebSecurityEngine.Get<IAuthenticationContextService>().GetCurrentService(); }
		}

		#endregion

		#region Methods

		public void Init(HttpApplication app)
		{
			// Because it is possible to override authentication on a folder-by-folder
			// basis, we can't enable / disable authentication at this stage.
			// We need to do it inside OnEnter.
			app.AuthenticateRequest += OnAuthenticateRequest;
			app.EndRequest += OnEndRequest;
		}

		protected virtual void OnAuthenticateRequest(object source, EventArgs eventArgs)
		{
			HttpApplication application = (HttpApplication) source;
			HttpContextBase context = new HttpContextWrapper(application.Context);

			IAuthenticationContextService authenticationContextService = WebSecurityEngine.Get<IAuthenticationContextService>();
			AuthenticationSection authenticationConfig = System.Web.Configuration.WebConfigurationManager.GetSection("zeus/authentication") as AuthenticationSection;
			string locationPath = context.Request.Path.ToLower();
			if (authenticationConfig != null && !authenticationContextService.ContainsLocation(locationPath))
			{
				AuthenticationLocation location = authenticationConfig.ToAuthenticationLocation();
				location.Path = locationPath;
				authenticationContextService.AddLocation(location);
			}

			if (!CurrentAuthenticationService.Enabled)
				return;

			OnAuthenticate(new AuthenticationEventArgs(context));
			if (CurrentAuthenticationService.AccessingLoginPage())
				context.SkipAuthorization = true;

			//if (!context.SkipAuthorization)
			//	context.SkipAuthorization = AssemblyResourceLoader.IsValidWebResourceRequest(context);

			_onEnterCalled = true;
		}

		protected virtual void OnAuthenticate(AuthenticationEventArgs e)
		{
			if (Authenticate != null)
				Authenticate(this, e);

			if (e.Context.User != null)
				return;

			if (e.User != null)
			{
				e.Context.User = e.User;
				return;
			}

			FormsAuthenticationTicket tOld;
			try
			{
				tOld = CurrentAuthenticationService.ExtractTicketFromCookie();
			}
			catch
			{
				tOld = null;
			}

			if (tOld == null || tOld.Expired)
				return;

			FormsAuthenticationTicket ticket = tOld;
			if (CurrentAuthenticationService.Config.SlidingExpiration)
				ticket = CurrentAuthenticationService.RenewTicketIfOld(tOld);

			User membershipUser = null;
			try
			{
				membershipUser = WebSecurityEngine.Get<ICredentialService>().GetUser(ticket.Name);	
			}
			catch
			{
			}
			if (membershipUser == null)
				return;

			e.Context.User = new WebPrincipal(membershipUser, ticket);

			if (ticket == tOld)
				return;

			HttpCookie cookie = null;
			if (!ticket.CookiePath.Equals("/"))
			{
				cookie = e.Context.Request.Cookies[CurrentAuthenticationService.Config.Name];
				if (cookie != null)
					cookie.Path = ticket.CookiePath;
			}

			CurrentAuthenticationService.CreateOrUpdateCookieFromTicket(ticket, cookie);
		}

		protected virtual void OnEndRequest(object source, EventArgs eventArgs)
		{
			if (!_onEnterCalled)
				return;

			_onEnterCalled = false;

			HttpApplication application = (HttpApplication) source;
			HttpContext context = application.Context;
			if (context.Response.StatusCode != 0x191)
				return;

			// Add new ReturnUrl parameter, which will remove any existing parameter of this name.
			Url redirectUrl = new Url(CurrentAuthenticationService.LoginUrl);
			redirectUrl.SetQueryParameter("ReturnUrl", new Url(context.Request.Url).PathAndQuery);
			context.Response.Redirect(redirectUrl.ToString(), false);
		}

		public void Dispose()
		{

		}

		#endregion
	}
}