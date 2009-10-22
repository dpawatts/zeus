using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Pagination;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
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
	}
}