using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Isis.Web;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(Forum))]
	public class ForumController : BaseForumController<Forum, IForumViewData>
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
			PopulateViewData(p);
			return View("Index");
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

		private void PopulateViewData(int? p)
		{
			TypedViewData.CurrentUserIsForumModerator = (CurrentUser != null && CurrentItem.Moderator != null && CurrentUser.Username == CurrentItem.Moderator.User.Username);
			TypedViewData.Topics = CurrentItem.GetChildren<Topic>().OrderByDescending(t => t.Updated).OrderBy(t => t.Sticky ? 0 : 1).AsPagination(p ?? 1, CurrentMessageBoard.TopicsPerPage);
		}
	}

	public interface IForumViewData : IBaseForumViewData<Forum>
	{
		IPagination<Topic> Topics { get; set; }
		bool CurrentUserIsForumModerator {get;set;}
	}
}