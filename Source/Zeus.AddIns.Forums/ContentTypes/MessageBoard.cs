using Coolite.Ext.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Globalization;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType("Message Board")]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	[Template("~/UI/Views/Forums/MessageBoard.aspx")]
	[Template("search", "~/UI/Views/Forums/SearchResults.aspx")]
	[Translatable(false)]
	public class MessageBoard : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Comments); }
		}

		public string PostUrl
		{
			get { return new Url(Url).AppendSegment("post"); }
		}

		[ContentProperty("Topics Per Page", 40, EditorContainerName = "Content")]
		public virtual int TopicsPerPage
		{
			get { return GetDetail("TopicsPerPage", 20); }
			set { SetDetail("TopicsPerPage", value); }
		}

		[ContentProperty("Posts Per Page", 50, EditorContainerName = "Content")]
		public virtual int PostsPerPage
		{
			get { return GetDetail("PostsPerPage", 20); }
			set { SetDetail("PostsPerPage", value); }
		}

		[ContentProperty("Search Results Per Page", 60, EditorContainerName = "Content")]
		public virtual int SearchResultsPerPage
		{
			get { return GetDetail("SearchResultsPerPage", 20); }
			set { SetDetail("SearchResultsPerPage", value); }
		}
	}
}