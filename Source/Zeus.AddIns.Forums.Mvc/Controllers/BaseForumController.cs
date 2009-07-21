using System;
using System.Web.Mvc;
using Isis.Web;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.AddIns.Forums.Services;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.Controllers;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	public abstract class BaseForumController<T> : ZeusController<T>
		where T : BasePage
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

		protected string PostUrl
		{
			get { return new Url(CurrentMessageBoard.Url).AppendSegment("post"); }
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			BaseForumViewModel<T> viewModel = filterContext.Controller.ViewData.Model as BaseForumViewModel<T>;
			if (viewModel != null)
			{
				viewModel.ForumService = ForumService;
				viewModel.CurrentMember = CurrentMember;
				viewModel.SearchUrl = SearchUrl;	
			}
			base.OnActionExecuted(filterContext);
		}
	}
}