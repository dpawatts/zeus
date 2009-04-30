using Castle.Core;
using Isis.ComponentModel;
using Isis.Web.Security;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public class PermissionDeniedHandler : IStartable
	{
		private readonly ISecurityEnforcer _securityEnforcer;

		public PermissionDeniedHandler(ISecurityEnforcer securityEnforcer)
		{
			_securityEnforcer = securityEnforcer;
		}

		private void securityEnforcer_AuthorizationFailed(object sender, CancelItemEventArgs e)
		{
			e.Cancel = true;
			IoC.Resolve<IAuthenticationContextService>().GetCurrentService().RedirectToLoginPage();
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