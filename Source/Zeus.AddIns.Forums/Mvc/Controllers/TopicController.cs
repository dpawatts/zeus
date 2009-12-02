using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.BaseLibrary.Web;
using Zeus.Web;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(Topic), AreaName = ForumsWebPackage.AREA_NAME)]
	public class TopicController : BaseForumController<Topic>
	{
		protected override MessageBoard CurrentMessageBoard
		{
			get { return CurrentItem.Forum.MessageBoard; }
		}

		protected override string SearchUrl
		{
			get { return new Url(CurrentMessageBoard.Url).AppendSegment("search").AppendQuery("t", CurrentItem.ID); }
		}

		[ActionName("NotUsed")]
		public override ActionResult Index()
		{
			throw new InvalidOperationException();
		}

		[HttpGet]
		public ActionResult Index(int? p, int? post)
		{
			var allPosts = CurrentItem.GetChildren<Post>().ToList();
			var viewModel = new TopicViewModel(CurrentItem, allPosts.AsPagination(p ?? 1, CurrentMessageBoard.PostsPerPage));

			// Increment view count.
			++CurrentItem.ViewCount;
			Context.Persister.Save(CurrentItem);

			// If a specific post is in the querystring, make sure we're on the correct page.
			if (post != null)
			{
				// Find page.
				int pageNumber = (allPosts.IndexOf(Context.Persister.Get<Post>(post.Value)) / CurrentMessageBoard.PostsPerPage) + 1;
				if (pageNumber != (p ?? 1))
					return Redirect(new Url(CurrentItem.Url).AppendQuery("p", pageNumber).SetFragment("post" + post));
			}

			return View(viewModel);
		}

		[HttpGet]
		public ActionResult Reply()
		{
			return View("Posting", GetPostViewModel());
		}

		[HttpGet]
		public ActionResult Quote(int p)
		{
			Post currentPost = Context.Persister.Get<Post>(p);

			PostingViewModel viewModel = GetPostViewModel();
			viewModel.Message = "[quote]" + currentPost.Message + "[/quote]";
			return View("Posting", viewModel);
		}

		private PostingViewModel GetPostViewModel()
		{
			PostingViewModel viewModel = new PostingViewModel(CurrentMessageBoard, new Url(CurrentItem.Url).AppendSegment("reply"));
			viewModel.CurrentTopicPosts = CurrentItem.GetChildren<Post>();
			viewModel.Subject = CurrentItem.Title;
			viewModel.TopicSummaryVisible = true;
			return viewModel;
		}

		[HttpPost]
		public ActionResult ReplyPreview(PostFormViewModel form)
		{
			PostingViewModel viewModel = new PostingViewModel(CurrentMessageBoard, new Url(CurrentItem.Url).AppendSegment("reply"));
			viewModel.CurrentTopicPosts = CurrentItem.GetChildren<Post>();
			viewModel.PreviewMessage = BBCodeHelper.ConvertToHtml(form.Message);
			viewModel.PreviewVisible = true;
			return View("Posting", viewModel);
		}

		[HttpPost]
		public ActionResult Reply(PostFormViewModel form)
		{
			if (!ModelState.IsValid)
				return View();

			Post post = ForumService.CreateReply(CurrentItem, CurrentMember, form.Subject, form.Message);
			return Redirect(post.Url);
		}
	}
}