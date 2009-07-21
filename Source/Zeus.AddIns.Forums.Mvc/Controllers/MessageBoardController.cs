using System;
using System.Linq;
using System.Web.Mvc;
using Isis.ExtensionMethods;
using Isis.Web;
using MvcContrib.Pagination;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(MessageBoard), AreaName = ForumsWebPackage.AREA_NAME)]
	public class MessageBoardController : BaseForumController<MessageBoard>
	{
		protected override MessageBoard CurrentMessageBoard
		{
			get { return CurrentItem; }
		}

		protected override string SearchUrl
		{
			get { return new Url(CurrentMessageBoard.Url).AppendSegment("search"); }
		}

		#region Index action

		public override ActionResult Index()
		{
			return View(new MessageBoardViewModel(CurrentItem, CurrentItem.GetChildren<Forum>()));
		}

		#endregion

		#region Search action

		public ActionResult Search(int? f, int? t, string q, int? p)
		{
			var results = Find.EnumerateAccessibleChildren(CurrentMessageBoard).OfType<Post>();
			if (f != null)
				results = results.Where(post => post.Parent.Parent.ID == f.Value);
			if (t != null)
				results = results.Where(post => post.Parent.ID == t.Value);
			results = results.Where(post => post.Title.Contains(q, StringComparison.InvariantCultureIgnoreCase) || post.Message.Contains(q, StringComparison.InvariantCultureIgnoreCase));

			return View(new MessageBoardSearchResultsViewModel(CurrentItem)
			{
				SearchResults = results.AsPagination(p ?? 1, CurrentMessageBoard.SearchResultsPerPage),
				SearchResultCount = results.Count(),
				SearchText = q
			});
		}

		#endregion

		#region Posting actions

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Post(PostingMode mode, int? f, int? t, int? p)
		{
			MessageBoardPostViewModel viewModel = new MessageBoardPostViewModel(CurrentItem);

			if (t != null)
				viewModel.CurrentTopic = Context.Persister.Get<Topic>(t.Value);

			switch (mode)
			{
				case PostingMode.Edit:
					{
						Post currentPost = Context.Persister.Get<Post>(p.Value);
						viewModel.Subject = currentPost.Title;
						viewModel.Message = currentPost.Message;
					}
					break;
				case PostingMode.Reply:
					{
						Topic currentTopic = Context.Persister.Get<Topic>(t.Value);
						viewModel.Subject = currentTopic.Title;
					}
					break;
				case PostingMode.Quote:
					{
						Topic currentTopic = Context.Persister.Get<Topic>(t.Value);
						Post currentPost = Context.Persister.Get<Post>(p.Value);
						viewModel.Subject = currentTopic.Title;
						viewModel.Message = "[quote]" + currentPost.Message + "[/quote]";
					}
					break;
			}

			SetVisibility(viewModel, mode);

			return View(viewModel);
		}

		private static void SetVisibility(MessageBoardPostViewModel viewModel, PostingMode mode)
		{
			switch (mode)
			{
				case PostingMode.Reply:
				case PostingMode.Quote:
					viewModel.TopicSummaryVisible = true;
					break;
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult PostPreview(PostingMode? mode, int? f, int? t, int? p, string subject, string message)
		{
			if (mode == null)
				throw new InvalidOperationException();

			MessageBoardPostViewModel viewModel = new MessageBoardPostViewModel(CurrentItem);

			if (t != null)
				viewModel.CurrentTopic = Context.Persister.Get<Topic>(t.Value);

			SetVisibility(viewModel, mode.Value);

			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
			{
				if (string.IsNullOrEmpty(subject))
					ModelState.AddModelError("subject", "Subject is required");
				if (string.IsNullOrEmpty(message))
					ModelState.AddModelError("message", "Message is required");
			}
			else
			{
				viewModel.PreviewMessage = BBCodeHelper.ConvertToHtml(message);
				viewModel.PreviewVisible = true;
			}

			return View(viewModel);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Post(PostingMode? mode, int? f, int? t, int? p, string subject, string message)
		{
			if (mode == null)
				throw new InvalidOperationException();

			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
			{
				MessageBoardPostViewModel viewModel = new MessageBoardPostViewModel(CurrentItem);

				if (string.IsNullOrEmpty(subject))
					ModelState.AddModelError("subject", "Subject is required");
				if (string.IsNullOrEmpty(message))
					ModelState.AddModelError("message", "Message is required");

				if (t != null)
					viewModel.CurrentTopic = Context.Persister.Get<Topic>(t.Value);

				SetVisibility(viewModel, mode.Value);

				return View(viewModel);
			}

			Forum currentForum = Context.Persister.Get<Forum>(f.Value);

			string redirectUrl = string.Empty;
			switch (mode)
			{
				case PostingMode.NewTopic:
					{
						Topic topic = ForumService.CreateTopic(currentForum, CurrentMember, subject, message);
						redirectUrl = topic.Url;
					}
					break;
				case PostingMode.Edit:
					{
						Post currentPost = Context.Persister.Get<Post>(p.Value);
						Post post = ForumService.EditPost(currentPost, CurrentMember, subject, message);
						redirectUrl = post.Url;
					}
					break;
				case PostingMode.Reply:
				case PostingMode.Quote:
					{
						Topic currentTopic = Context.Persister.Get<Topic>(t.Value);
						Post post = ForumService.CreateReply(currentTopic, CurrentMember, subject, message);
						redirectUrl = post.Url;
					}
					break;
			}

			return Redirect(redirectUrl);
		}

		public enum PostingMode
		{
			NewTopic,
			Edit,
			Reply,
			Quote
		}

		#endregion
	}
}