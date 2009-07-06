using System.Collections.Generic;
using System.Linq;
using Isis.Web;
using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.Globalization;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(Forum))]
	[Template("~/UI/Views/Forums/Topic.aspx")]
	[Translatable(false)]
	public class Topic : BasePage
	{
		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(Forum), "Zeus.AddIns.Forums.Web.Resources.comment.png"); }
		}

		[LinkedItemDropDownListEditor("Author", 20, TypeFilter = typeof(Member))]
		public virtual Member Author
		{
			get { return GetDetail<Member>("Author", null); }
			set { SetDetail("Author", value); }
		}

		[CheckBoxEditor("Sticky?", "", 30)]
		public virtual bool Sticky
		{
			get { return GetDetail("Sticky", false); }
			set { SetDetail("Sticky", value); }
		}

		public virtual int PostCount
		{
			get { return Posts.Count(); }
		}

		public virtual int ReplyCount
		{
			get { return PostCount - 1; }
		}

		/// <summary>
		/// Here as an optimisation so we don't need to query the DB
		/// every time we display the forum list page
		/// </summary>
		public virtual int ViewCount
		{
			get { return GetDetail("ViewCount", 0); }
			set { SetDetail("ViewCount", value); }
		}

		public virtual Post LastPost
		{
			get { return Posts.Last(); }
		}

		public Forum Forum
		{
			get { return (Forum) Parent; }
		}

		public string ReplyUrl
		{
			get
			{
				return new Url(Forum.MessageBoard.NewPostPage.Url)
					.AppendQuery("f", Forum.ID)
					.AppendQuery("t", ID)
					.AppendQuery("mode", "reply")
					.ToString();
			}
		}

		public IEnumerable<Post> Posts
		{
			get { return GetChildren<Post>(); }
		}

		public int PageCount
		{
			get
			{
				int postsCount = Posts.Count();
				if (postsCount <= 0)
					return 1;

				int temp = postsCount + Forum.MessageBoard.PostsPerPage - 1;
				if (temp < 0)
					return 1;

				return temp / Forum.MessageBoard.PostsPerPage;
			}
		}
	}
}