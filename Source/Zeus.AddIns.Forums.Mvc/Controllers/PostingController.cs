using System;
using System.Web.Mvc;
using Isis.Web;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(NewPost))]
	public class PostingController : BaseForumController<NewPost, IPostingViewData>
	{
		protected override MessageBoard CurrentMessageBoard
		{
			get { return Context.Persister.Get<Forum>(Convert.ToInt32(HttpContext.Request["f"])).MessageBoard; }
		}

		protected override string SearchUrl
		{
			get { return new Url(CurrentMessageBoard.Url).AppendSegment("search"); }
		}

		[ActionName("NotUsed")]
		public override ActionResult Index()
		{
			throw new InvalidOperationException();
		}

		public ActionResult Index(PostingMode mode, int? f, int? t, int? p)
		{
			if (t != null)
				TypedViewData.CurrentTopic = Context.Persister.Get<Topic>(t.Value);

			switch (mode)
			{
				case PostingMode.Edit:
					{
						Post currentPost = Context.Persister.Get<Post>(p.Value);
						ViewData["subject"] = currentPost.Title;
						ViewData["message"] = currentPost.Message;
					}
					break;
				case PostingMode.Reply:
					{
						Topic currentTopic = Context.Persister.Get<Topic>(t.Value);
						ViewData["subject"] = currentTopic.Title;
					}
					break;
				case PostingMode.Quote:
					{
						Topic currentTopic = Context.Persister.Get<Topic>(t.Value);
						Post currentPost = Context.Persister.Get<Post>(p.Value);
						ViewData["subject"] = currentTopic.Title;
						ViewData["message"] = "[quote]" + currentPost.Message + "[/quote]";
					}
					break;
			}

			SetVisibility(mode);

			return View("Index");
		}

		private void SetVisibility(PostingMode mode)
		{
			switch (mode)
			{
				case PostingMode.Reply:
				case PostingMode.Quote:
					TypedViewData.TopicSummaryVisible = true;
					break;
			}
		}

		public ActionResult Preview(PostingMode? mode, int? f, int? t, int? p, string subject, string message)
		{
			if (mode == null)
				throw new InvalidOperationException();

			if (t != null)
				TypedViewData.CurrentTopic = Context.Persister.Get<Topic>(t.Value);

			SetVisibility(mode.Value);

			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
			{
				if (string.IsNullOrEmpty(subject))
					ModelState["subject"].Errors.Add("Subject is required");
				if (string.IsNullOrEmpty(message))
					ModelState["message"].Errors.Add("Message is required");
			}
			else
			{
				TypedViewData.PreviewMessage = BBCodeHelper.ConvertToHtml(message);
				TypedViewData.PreviewVisible = true;
			}

			return View("Index");
		}

		public ActionResult Submit(PostingMode? mode, int? f, int? t, int? p, string subject, string message)
		{
			if (mode == null)
				throw new InvalidOperationException();

			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
			{
				if (string.IsNullOrEmpty(subject))
					ModelState["subject"].Errors.Add("Subject is required");
				if (string.IsNullOrEmpty(message))
					ModelState["message"].Errors.Add("Message is required");

				if (t != null)
					TypedViewData.CurrentTopic = Context.Persister.Get<Topic>(t.Value);

				SetVisibility(mode.Value);

				return View("Index");
			}
			else
			{
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
		}

		public enum PostingMode
		{
			NewTopic,
			Edit,
			Reply,
			Quote
		}
	}

	public interface IPostingViewData : IBaseForumViewData<NewPost>
	{
		Topic CurrentTopic { get; set; }
		bool TopicSummaryVisible { get; set; }
		string PreviewMessage { get; set; }
		bool PreviewVisible { get; set; }
	}
}