using System;
using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.AddIns.Forums.Services;
using Zeus.BaseLibrary.Web;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web.Mvc;
using Zeus.Web.Security;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[ModelStateToTempData]
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

		[HttpPost]
		public ActionResult Login(string username, string password)
		{
			if (!ModelState.IsValid || !Engine.Resolve<ICredentialService>().ValidateUser(username, password))
			{
				TempData["ForumLogin.Failed"] = "Invalid username or password";
				return Redirect(CurrentItem.Url + "#fWrongLogin");
			}

			Engine.Resolve<IAuthenticationContextService>().GetCurrentService().SetAuthCookie(username, false);
			return Redirect(CurrentItem.Url);
		}

		[HttpGet]
		public ActionResult Logout()
		{
			Engine.Resolve<IAuthenticationContextService>().GetCurrentService().SignOut();
			return Redirect(CurrentItem.Url);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			IBaseForumViewModel viewModel = filterContext.Controller.ViewData.Model as IBaseForumViewModel;
			if (viewModel != null)
			{
				viewModel.CurrentMember = CurrentMember;
				viewModel.CurrentMessageBoard = CurrentMessageBoard;
				viewModel.ForumService = ForumService;
				viewModel.LoggedIn = User.Identity.IsAuthenticated;
				viewModel.SearchUrl = SearchUrl;	
			}
			base.OnActionExecuted(filterContext);
		}
	}
}