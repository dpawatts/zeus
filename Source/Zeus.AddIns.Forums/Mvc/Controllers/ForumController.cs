using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.BaseLibrary.Web;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(Forum), AreaName = ForumsWebPackage.AREA_NAME)]
	public class ForumController : BaseForumController<Forum>
	{
		public ForumController(IWebSecurityService webSecurityService)
			: base(webSecurityService)
		{
		}

		protected override MessageBoard CurrentMessageBoard
		{
			get { return CurrentItem.MessageBoard; }
		}

		protected override string SearchUrl
		{
			get { return new Url(CurrentMessageBoard.Url).AppendSegment("search").AppendQuery("f", CurrentItem.ID); }
		}

		[ActionName("NotUsed")]
		public override ActionResult Index()
		{
			throw new InvalidOperationException();
		}

		public ActionResult Index(int? p)
		{
			ForumViewModel viewModel = new ForumViewModel(CurrentItem,
				CurrentItem.GetChildren<Topic>().OrderByDescending(t => t.Updated).OrderBy(t => t.Sticky ? 0 : 1).AsPagination(p ?? 1, CurrentMessageBoard.TopicsPerPage),
				(CurrentUser != null && CurrentItem.Moderator != null && CurrentUser.Username == CurrentItem.Moderator.User.Username),
				new Url(CurrentItem.Url).AppendSegment("newTopic"));
			return View(viewModel);
		}

		public ActionResult ToggleStickyTopic(int? p, int? t)
		{
			Topic topic = Context.Persister.Get<Topic>(t.Value);
			ForumService.ToggleTopicStickiness(topic, CurrentMember);

			return RedirectToAction("Index");
		}

		public ActionResult TrashTopic(int? p, int? t)
		{
			Topic topic = Context.Persister.Get<Topic>(t.Value);
			ForumService.TrashTopic(topic, CurrentMember);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult NewTopic()
		{
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingNewTopicNotLoggedInViewModel(CurrentItem));

			var viewModel = new NewTopicPostingViewModel(CurrentItem, new Url(CurrentItem.Url).AppendSegment("newTopic"))
			{
				CanEditSubject = true
			};
			return View("Posting", viewModel);
		}

		[HttpPost]
		public ActionResult NewTopic(PostFormViewModel postingForm)
		{
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingNewTopicNotLoggedInViewModel(CurrentItem));

			if (!ModelState.IsValid)
				return NewTopic();

			Topic topic = ForumService.CreateTopic(CurrentItem, CurrentMember, postingForm.Subject, postingForm.Message);
			return Redirect(topic.Url);
		}
	}
}