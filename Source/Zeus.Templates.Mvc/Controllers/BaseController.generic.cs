using Isis.Web.Security;
using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc;

namespace Zeus.Templates.Mvc.Controllers
{
	public abstract class BaseController<T, TViewData> : ContentController<T, TViewData>
		where T : BasePage
		where TViewData : class, IViewData<T>
	{
		public IUser CurrentUser
		{
			get
			{
				WebPrincipal webPrincipal = User as WebPrincipal;
				return (webPrincipal != null) ? webPrincipal.MembershipUser : null;
			}
		}

		protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
		{
			TypedViewData.CurrentPage = CurrentItem;
			base.OnActionExecuting(filterContext);
		}
	}

	public interface IViewData<TPage>
		where TPage : BasePage
	{
		TPage CurrentPage { get; set; }
	}
}