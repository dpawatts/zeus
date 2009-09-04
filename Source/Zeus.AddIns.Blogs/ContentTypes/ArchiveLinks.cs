using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Archive Links")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class ArchiveLinks : BaseContentItem
	{
		[ContentProperty("Blog", 200)]
		public Blog Blog
		{
			get { return GetDetail<Blog>("Blog", null); }
			set { SetDetail("Blog", value); }
		}
	}
}