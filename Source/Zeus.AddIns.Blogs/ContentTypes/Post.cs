using System;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(Blog), typeof(BlogMonth))]
	public class Post : BaseBlogPage
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
	}
}