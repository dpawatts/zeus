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
	[Controls(typeof(Topic), AreaName = ForumsWebPackage.AREA_NAME)]
	public class TopicController : BaseForumController<Topic>
	{
		public TopicController(IWebSecurityService webSecurityService)
			: base(webSecurityService)
		{
		}

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
			var viewModel = new TopicViewModel(CurrentItem, allPosts.AsPagination(p ?? 1, CurrentMessageBoard.PostsPerPage),
				new Url(CurrentItem.Forum.Url).AppendSegment("newTopic"));

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
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingReplyTopicNotLoggedInViewModel(CurrentItem));

			ReplyTopicPostingViewModel viewModel = GetPostViewModel();
			viewModel.Subject = CurrentItem.Title;
			return View("Posting", viewModel);
		}

		[HttpGet]
		public ActionResult Quote(int p)
		{
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingReplyTopicNotLoggedInViewModel(CurrentItem));

			Post currentPost = Context.Persister.Get<Post>(p);

			ReplyTopicPostingViewModel viewModel = GetPostViewModel();
			viewModel.Subject = CurrentItem.Title;
			viewModel.Message = "[quote]" + currentPost.Message + "[/quote]";
			return View("Posting", viewModel);
		}

		private ReplyTopicPostingViewModel GetPostViewModel()
		{
			ReplyTopicPostingViewModel viewModel = new ReplyTopicPostingViewModel(CurrentItem,
				new Url(CurrentItem.Url).AppendSegment("reply"),
				new Url(CurrentItem.Forum.Url).AppendSegment("newTopic"),
				CurrentItem.GetChildren<Post>());
			viewModel.Subject = CurrentItem.Title;
			return viewModel;
		}

		[HttpPost]
		public ActionResult Reply(PostFormViewModel postingForm)
		{
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingReplyTopicNotLoggedInViewModel(CurrentItem));

			if (!ModelState.IsValid)
				return View();

			Post post = ForumService.CreateReply(CurrentItem, CurrentMember, postingForm.Subject, postingForm.Message);
			return Redirect(post.Url);
		}

		[HttpGet]
		public ActionResult Edit(int p)
		{
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingReplyTopicNotLoggedInViewModel(CurrentItem));

			Post currentPost = Context.Persister.Get<Post>(p);

			ReplyTopicPostingViewModel viewModel = GetPostViewModel();
			viewModel.PostingFormUrl = new Url(CurrentItem.Url).AppendSegment("edit");
			viewModel.Subject = currentPost.Title;
			viewModel.Message = currentPost.Message;
			return View("Posting", viewModel);
		}

		[HttpPost]
		public ActionResult Edit(int p, PostFormViewModel postingForm)
		{
			if (!CurrentMessageBoard.AllowAnonymousPosts && !User.Identity.IsAuthenticated)
				return View("PostingNotLoggedIn", new PostingReplyTopicNotLoggedInViewModel(CurrentItem));

			if (!ModelState.IsValid)
				return View();

			Post currentPost = Context.Persister.Get<Post>(p);
			ForumService.EditPost(currentPost, CurrentMember, postingForm.Subject, postingForm.Message);
			return Redirect(currentPost.Url);
		}
	}
}