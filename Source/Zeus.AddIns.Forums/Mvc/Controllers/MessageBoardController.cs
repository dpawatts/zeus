using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.BaseLibrary.Web;
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

		public override ActionResult Index()
		{
			return View(new MessageBoardViewModel(CurrentItem, CurrentItem.GetChildren<Forum>()));
		}

		[HttpGet]
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
	}
}