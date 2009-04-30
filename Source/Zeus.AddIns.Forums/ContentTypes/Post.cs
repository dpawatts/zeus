using System;
using System.Web.UI.WebControls;
using Isis.Web;
using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(Topic))]
	public class Post : ContentItem
	{
		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(Forum), "Zeus.AddIns.Forums.Web.Resources.comment.png"); }
		}

		[TextBoxEditor("Subject", 10)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[LinkedItemDropDownListEditor("Author", 20, TypeFilter = typeof(Member))]
		public virtual Member Author
		{
			get { return GetDetail<Member>("Author", null); }
			set { SetDetail("Author", value); }
		}

		[DateEditor("Date", 30)]
		public override DateTime Created
		{
			get { return base.Created; }
			set { base.Created = value; }
		}

		[TextBoxEditor("Message", 40, TextMode = TextBoxMode.MultiLine)]
		public virtual string Message
		{
			get { return GetDetail("Message", string.Empty); }
			set { SetDetail("Message", value); }
		}

		public string HtmlMessage
		{
			get { return BBCodeHelper.ConvertToHtml(Message); }
		}

		public Topic Topic
		{
			get { return (Topic) Parent; }
		}

		public string EditUrl
		{
			get
			{
				return new Url(Topic.Forum.MessageBoard.NewPostPage.Url)
					.AppendQuery("f", Topic.Forum.ID)
					.AppendQuery("t", Topic.ID)
					.AppendQuery("p", ID)
					.AppendQuery("mode", "edit")
					.ToString();
			}
		}

		public string QuoteUrl
		{
			get
			{
				return new Url(Topic.Forum.MessageBoard.NewPostPage.Url)
					.AppendQuery("f", Topic.Forum.ID)
					.AppendQuery("t", Topic.ID)
					.AppendQuery("p", ID)
					.AppendQuery("mode", "quote")
					.ToString();
			}
		}

		public override string Url
		{
			get { return new Url(Topic.Url).AppendQuery("post", ID).ToString(); }
		}
	}
}