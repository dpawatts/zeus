using System;
using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Services;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.Controllers;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	public abstract class BaseForumController<T, TViewData> : BaseController<T, TViewData>
		where T : BasePage
		where TViewData : class, IBaseForumViewData<T>
	{
		protected IForumService ForumService
		{
			get { return Context.Current.Resolve<IForumService>(); }
		}

		protected abstract MessageBoard CurrentMessageBoard { get; }

		protected Member CurrentMember
		{
			get { return ForumService.GetMember(CurrentMessageBoard, CurrentUser, true); }
		}

		protected abstract string SearchUrl { get; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			TypedViewData.ForumService = ForumService;
			TypedViewData.CurrentMember = CurrentMember;
			TypedViewData.SearchUrl = SearchUrl;
			base.OnActionExecuting(filterContext);
		}
	}

	public interface IBaseForumViewData<T> : IViewData<T>
		where T : BasePage
	{
		IForumService ForumService { get; set; }
		Member CurrentMember { get; set; }
		string SearchUrl { get; set; }
	}
}