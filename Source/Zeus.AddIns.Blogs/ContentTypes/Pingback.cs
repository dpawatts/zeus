namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Blog Post Pingback")]
	public class Pingback : FeedbackItem
	{
		[ContentProperty("Source Page Title", 200)]
		public virtual string SourcePageTitle
		{
			get { return GetDetail("SourcePageTitle", string.Empty); }
			set { SetDetail("SourcePageTitle", value); }
		}

		[ContentProperty("Source URL", 210)]
		public virtual string SourceUrl
		{
			get { return GetDetail("SourceUrl", string.Empty); }
			set { SetDetail("SourceUrl", value); }
		}
	}
}