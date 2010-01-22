using Ext.Net;
using Zeus.BaseLibrary.Web;
using Zeus.ContentTypes;
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
	public class MessageBoard : BasePage, ISelfPopulator
	{
		private const string MemberContainerName = "members";

		public MemberContainer Members
		{
			get { return GetChild(MemberContainerName) as MemberContainer; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Comments); }
		}

		public string PostUrl
		{
			get { return new Url(Url).AppendSegment("post"); }
		}

		[ContentProperty("Allow Anonymous Posts", 39, EditorContainerName = "Content")]
		public virtual bool AllowAnonymousPosts
		{
			get { return GetDetail("AllowAnonymousPosts", false); }
			set { SetDetail("AllowAnonymousPosts", value); }
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

		[ContentProperty("Forgotten Password Page", 100)]
		public virtual PageContentItem ForgottenPasswordPage
		{
			get { return GetDetail<PageContentItem>("ForgottenPasswordPage", null); }
			set { SetDetail("ForgottenPasswordPage", value); }
		}

		[ContentProperty("Registration Page", 110)]
		public virtual PageContentItem RegistrationPage
		{
			get { return GetDetail<PageContentItem>("RegistrationPage", null); }
			set { SetDetail("RegistrationPage", value); }
		}

		void ISelfPopulator.Populate()
		{
			MemberContainer members = new MemberContainer
			{
				Name = MemberContainerName,
				Title = "Members"
			};
			members.AddTo(this);
		}
	}
}