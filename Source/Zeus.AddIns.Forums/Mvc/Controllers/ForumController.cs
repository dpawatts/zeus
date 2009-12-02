using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
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
			PostingViewModel viewModel = new PostingViewModel(CurrentMessageBoard, new Url(CurrentItem.Url).AppendSegment("newTopic"));
			return View("Posting", viewModel);
		}

		[HttpPost]
		public ActionResult NewTopicPreview(PostFormViewModel form)
		{
			PostingViewModel viewModel = new PostingViewModel(CurrentMessageBoard, new Url(CurrentItem.Url).AppendSegment("newTopic"));
			viewModel.PreviewMessage = BBCodeHelper.ConvertToHtml(form.Message);
			viewModel.PreviewVisible = true;
			return View("Posting", viewModel);
		}

		[HttpPost]
		public ActionResult NewTopic(PostFormViewModel form)
		{
			if (!ModelState.IsValid)
				return View();

			Topic topic = ForumService.CreateTopic(CurrentItem, CurrentMember, form.Subject, form.Message);
			return Redirect(topic.Url);
		}
	}
}