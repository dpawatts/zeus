using System;
using System.Collections.Generic;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.Services.Syndication;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Blog Post", "BlogPost")]
	[RestrictParents(typeof(Blog), typeof(BlogMonth))]
	public class Post : BaseBlogPage, ISyndicatable
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.CalendarViewDay); }
		}

		public override Blog CurrentBlog
		{
			get { return (GetParent() is BlogMonth) ? ((BlogMonth)GetParent()).CurrentBlog : (Blog)GetParent(); }
		}

		public virtual DateTime Date
		{
			get { return Published ?? Created; }
		}

		[DateEditor("Date Published", 100, IncludeTime = true)]
		public override DateTime? Published
		{
			get { return base.Published; }
			set { base.Published = value; }
		}

		DateTime ISyndicatable.Published
		{
			get { return (Published != null) ? Published.Value : DateTime.MinValue; }
		}

		[XhtmlStringContentProperty("Text", 200)]
		[HtmlTextBoxEditor(Required = true)]
		public virtual string Text
		{
			get { return GetDetail("Text", string.Empty); }
			set { SetDetail("Text", value); }
		}

		[ContentProperty("Excerpt", 210), TextBoxEditor(TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine, MaxLength = 300)]
		public virtual string Excerpt
		{
			get { return GetDetail("Excerpt", string.Empty); }
			set { SetDetail("Excerpt", value); }
		}

		[ContentProperty("Open For Comments", 215)]
		public virtual bool OpenForComments
		{
			get { return GetDetail("OpenForComments", true); }
			set { SetDetail("OpenForComments", value); }
		}

		[LinkedItemsCheckBoxListEditor("Categories", 220, typeof(Category))]
		public PropertyCollection Categories
		{
			get { return GetDetailCollection("Categories", true); }
		}

		public IEnumerable<FeedbackItem> Comments
		{
			get { return GetChildren<FeedbackItem>(); }
		}

		public IEnumerable<FeedbackItem> ApprovedComments
		{
			get { return Comments.Where(fi => fi.Status == FeedbackItemStatus.Approved); }
		}

		string ISyndicatable.Summary
		{
			get { return Text; }
		}

		public string GetExcerpt()
		{
			if (!string.IsNullOrEmpty(Excerpt))
				return "<p>" + Excerpt + "</p>";

			if (!string.IsNullOrEmpty(Text))
				return Text.Truncate(300, true);

			return "<p>" + Title + "</p>";
		}
	}
}