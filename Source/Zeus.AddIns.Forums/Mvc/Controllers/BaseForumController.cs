using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.AddIns.Forums.Services;
using Zeus.BaseLibrary.Web;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web.Mvc.ActionFilters;
using Zeus.Web.Security;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[ImportModelStateFromTempData]
	public abstract class BaseForumController<T> : ZeusController<T>
		where T : BasePage
	{
		private readonly IWebSecurityService _webSecurityService;

		protected BaseForumController(IWebSecurityService webSecurityService)
		{
			_webSecurityService = webSecurityService;
		}

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
		[ExportModelStateToTempData]
		public ActionResult Login(string username, string password)
		{
			if (!ModelState.IsValid || !_webSecurityService.ValidateUser(username, password))
			{
				ModelState.AddModelError("ForumLogin.Failed", "Invalid username or password");
				return RedirectToParentPage("#fWrongLogin");
			}

			_webSecurityService.SetAuthCookie(username, false);
			return RedirectToParentPage();
		}

		[HttpGet]
		public ActionResult Logout()
		{
			_webSecurityService.SignOut();
			return RedirectToParentPage();
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