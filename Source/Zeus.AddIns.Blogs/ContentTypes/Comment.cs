using Zeus.ContentProperties;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Blog Post Comment")]
	public class Comment : FeedbackItem
	{
		[ContentProperty("Author Name", 200)]
		public virtual string AuthorName
		{
			get { return GetDetail("AuthorName", string.Empty); }
			set { SetDetail("AuthorName", value); }
		}

		[ContentProperty("Author URL", 210)]
		public virtual string AuthorUrl
		{
			get { return GetDetail("AuthorUrl", string.Empty); }
			set { SetDetail("AuthorUrl", value); }
		}

		[ContentProperty("Author Email", 215)]
		public virtual string AuthorEmail
		{
			get { return GetDetail("AuthorEmail", string.Empty); }
			set { SetDetail("AuthorEmail", value); }
		}

		[XhtmlStringContentProperty("Text", 220)]
		public virtual string Text
		{
			get { return GetDetail("Text", string.Empty); }
			set { SetDetail("Text", value); }
		}
	}
}