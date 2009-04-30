using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Isis.ExtensionMethods;
using Isis.Web;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.Web;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(MessageBoard))]
	public class MessageBoardController : BaseForumController<MessageBoard, IMessageBoardViewData>
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
			TypedViewData.Forums = CurrentItem.GetChildren<Forum>();
			return View("Index");
		}

		public ActionResult Search(int? f, int? t, string q, int? p)
		{
			var results = Find.EnumerateAccessibleChildren(CurrentMessageBoard).OfType<Post>();
			if (f != null)
				results = results.Where(post => post.Parent.Parent.ID == f.Value);
			if (t != null)
				results = results.Where(post => post.Parent.ID == t.Value);
			results = results.Where(post => post.Title.Contains(q, StringComparison.InvariantCultureIgnoreCase) || post.Message.Contains(q, StringComparison.InvariantCultureIgnoreCase));

			IMessageBoardSearchResultsViewData vd = GetCustomTypedViewData<IMessageBoardSearchResultsViewData>();
			vd.SearchResults = results.AsPagination(p ?? 1, CurrentMessageBoard.SearchResultsPerPage);
			vd.SearchResultCount = results.Count();
			vd.SearchText = q;

			return View("Search");
		}
	}

	public interface IMessageBoardViewData : IBaseForumViewData<MessageBoard>
	{
		IEnumerable<Forum> Forums { get; set; }
	}

	public interface IMessageBoardSearchResultsViewData : IBaseForumViewData<MessageBoard>
	{
		IPagination<Post> SearchResults { get; set; }
		int SearchResultCount { get; set; }
		string SearchText { get; set; }
	}
}