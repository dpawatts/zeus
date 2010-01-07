using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Category List Widget")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class CategoryListWidget : WidgetContentItem
	{
		[ContentProperty("Blog", 200)]
		public Blog Blog
		{
			get { return GetDetail<Blog>("Blog", null); }
			set { SetDetail("Blog", value); }
		}
	}
}