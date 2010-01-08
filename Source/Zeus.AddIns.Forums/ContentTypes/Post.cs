using System;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Design.Editors;
using Zeus.Globalization;
using Zeus.Integrity;
using Zeus.Templates.Services;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType(Name = "ForumPost")]
	[RestrictParents(typeof(Topic))]
	[Translatable(false)]
	public class Post : ContentItem
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Comment); }
		}

		[ContentProperty("Author", 20)]
		public virtual Member Author
		{
			get { return GetDetail<Member>("Author", null); }
			set { SetDetail("Author", value); }
		}

		[ContentProperty("Date", 30)]
		public override DateTime Created
		{
			get { return base.Created; }
			set { base.Created = value; }
		}

		[ContentProperty("Message", 40)]
		[TextBoxEditor(TextMode = TextBoxMode.MultiLine)]
		public virtual string Message
		{
			get { return GetDetail("Message", string.Empty); }
			set { SetDetail("Message", value); }
		}

		public string HtmlMessage
		{
			get { return Context.Current.Resolve<BBCodeService>().GetHtml(Message); }
		}

		[ContentProperty("Number", 50)]
		public virtual int Number
		{
			get { return GetDetail("Number", 1); }
			set { SetDetail("Number", value); }
		}

		public Topic Topic
		{
			get { return (Topic) Parent; }
		}

		public string EditUrl
		{
			get
			{
				return new Url(Topic.Forum.MessageBoard.PostUrl)
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
				return new Url(Topic.Forum.MessageBoard.PostUrl)
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