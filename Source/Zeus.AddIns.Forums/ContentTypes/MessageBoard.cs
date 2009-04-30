using System.Web.UI;
using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType("Message Board")]
	[RestrictParents(typeof(IMessageBoardContainer))]
	[Template("~/UI/Views/Forums/MessageBoard.aspx")]
	[Template("search", "~/UI/Views/Forums/SearchResults.aspx")]
	public class MessageBoard : BasePage
	{
		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(MessageBoard), "Zeus.AddIns.Forums.Web.Resources.comments.png"); }
		}

		[LinkedItemDropDownListEditor("New / Edit Post Page", 30, Required = true, TypeFilter = typeof(ContentItem))]
		public virtual ContentItem NewPostPage
		{
			get { return GetDetail<ContentItem>("NewPostPage", null); }
			set { SetDetail("NewPostPage", value); }
		}

		[TextBoxEditor("Topics Per Page", 40)]
		public virtual int TopicsPerPage
		{
			get { return GetDetail("TopicsPerPage", 20); }
			set { SetDetail("TopicsPerPage", value); }
		}

		[TextBoxEditor("Posts Per Page", 50)]
		public virtual int PostsPerPage
		{
			get { return GetDetail("PostsPerPage", 20); }
			set { SetDetail("PostsPerPage", value); }
		}

		[TextBoxEditor("Search Results Per Page", 60)]
		public virtual int SearchResultsPerPage
		{
			get { return GetDetail("SearchResultsPerPage", 20); }
			set { SetDetail("SearchResultsPerPage", value); }
		}

		/*/// <summary>
		/// Here as an optimisation so we don't need to query the DB
		/// every time we display the forum list page
		/// </summary>
		public virtual int TotalPosts
		{
			get { return GetDetail("TotalPosts", 0); }
			internal set { SetDetail("TotalPosts", value); }
		}

		/// <summary>
		/// Here as an optimisation so we don't need to query the DB
		/// every time we display the forum list page
		/// </summary>
		public virtual int TotalTopics
		{
			get { return GetDetail("TotalTopics", 0); }
			internal set { SetDetail("TotalTopics", value); }
		}*/
	}
}