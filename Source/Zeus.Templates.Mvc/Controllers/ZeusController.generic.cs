using Isis.Web.Security;
using Zeus.Web.Mvc;

namespace Zeus.Templates.Mvc.Controllers
{
	public abstract class ZeusController<T> : ContentController<T>
		where T : ContentItem
	{
		public IUser CurrentUser
		{
			get
			{
				WebPrincipal webPrincipal = User as WebPrincipal;
				return (webPrincipal != null) ? webPrincipal.MembershipUser : null;
			}
		}
	}
}