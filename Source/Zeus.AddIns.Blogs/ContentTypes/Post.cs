using System;
using Isis.ExtensionMethods;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.Services.Syndication;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Blog Post")]
	[RestrictParents(typeof(Blog), typeof(BlogMonth))]
	public class Post : BaseBlogPage, ISyndicatable
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Post), "Zeus.AddIns.Blogs.Icons.calendar_view_day.png"); }
		}

		public override Blog CurrentBlog
		{
			get { return (GetParent() is BlogMonth) ? ((BlogMonth)GetParent()).CurrentBlog : (Blog)GetParent(); }
		}

		public virtual DateTime Date
		{
			get { return Published ?? Created; }
		}

		[XhtmlStringContentProperty("Text", 200)]
		public virtual string Text
		{
			get { return GetDetail("Text", string.Empty); }
			set { SetDetail("Text", value); }
		}

		[ContentProperty("Excerpt", 210), TextBoxEditor(TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public virtual string Excerpt
		{
			get { return GetDetail("Excerpt", string.Empty); }
			set { SetDetail("Excerpt", value); }
		}

		[LinkedItemsCheckBoxListEditor("Categories", 220, typeof(Category))]
		public PropertyCollection Categories
		{
			get { return GetDetailCollection("Categories", true); }
		}

		string ISyndicatable.Summary
		{
			get { return Text; }
		}

		public string GetExcerpt()
		{
			if (!string.IsNullOrEmpty(Excerpt))
				return Excerpt;

			if (!string.IsNullOrEmpty(Text))
				return Text.Truncate(300, true);

			return Title;
		}
	}
}