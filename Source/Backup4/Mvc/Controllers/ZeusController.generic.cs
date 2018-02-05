using Zeus.Security;
using Zeus.Web.Mvc;
using Zeus.Web.Security;

namespace Zeus.Templates.Mvc.Controllers
{
	public abstract class ZeusController<T> : ContentController<T>
		where T : ContentItem
	{
		public User CurrentUser
		{
			get
			{
				WebPrincipal webPrincipal = User as WebPrincipal;
				return (webPrincipal != null) ? webPrincipal.MembershipUser : null;
			}
		}
	}
}