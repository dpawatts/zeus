using System;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Pagination;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.BaseLibrary.Web;
using Zeus.Web;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(Forum), AreaName = ForumsWebPackage.AREA_NAME)]
	public class ForumController : BaseForumController<Forum>
	{
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
			return View(new ForumViewModel(CurrentItem)
			{
				CurrentUserIsForumModerator = (CurrentUser != null && CurrentItem.Moderator != null && CurrentUser.Username == CurrentItem.Moderator.User.Username),
				Topics = CurrentItem.GetChildren<Topic>().OrderByDescending(t => t.Updated).OrderBy(t => t.Sticky ? 0 : 1).AsPagination(p ?? 1, CurrentMessageBoard.TopicsPerPage)
			});
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
	}
}