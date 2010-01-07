using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Archive Calendar Widget")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class ArchiveCalendarWidget : WidgetContentItem
	{
		[ContentProperty("Blog", 200)]
		public Blog Blog
		{
			get { return GetDetail<Blog>("Blog", null); }
			set { SetDetail("Blog", value); }
		}
	}
}