using Isis.Web.Security;
using Ninject;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public class PermissionDeniedHandler : IStartable
	{
		private readonly ISecurityEnforcer _securityEnforcer;
		private readonly IAuthenticationContextService _authenticationContextService;

		public PermissionDeniedHandler(ISecurityEnforcer securityEnforcer, IAuthenticationContextService authenticationContextService)
		{
			_securityEnforcer = securityEnforcer;
			_authenticationContextService = authenticationContextService;
		}

		private void securityEnforcer_AuthorizationFailed(object sender, CancelItemEventArgs e)
		{
			e.Cancel = true;
			_authenticationContextService.GetCurrentService().RedirectToLoginPage();
		}

		#region IStartable Members

		public void Start()
		{
			_securityEnforcer.AuthorizationFailed += securityEnforcer_AuthorizationFailed;
		}

		public void Stop()
		{
			_securityEnforcer.AuthorizationFailed -= securityEnforcer_AuthorizationFailed;
		}

		#endregion
	}
}