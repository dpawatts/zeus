using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Recent Blog Posts")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class RecentPosts : BaseContentItem
	{
		[ContentProperty("Blog", 200)]
		public Blog Blog
		{
			get { return GetDetail<Blog>("Blog", null); }
			set { SetDetail("Blog", value); }
		}

		[ContentProperty("# To Show", 210)]
		public int NumberToShow
		{
			get { return GetDetail("NumberToShow", 3); }
			set { SetDetail("NumberToShow", value); }
		}
	}
}