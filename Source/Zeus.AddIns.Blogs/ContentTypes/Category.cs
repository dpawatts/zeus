using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Category", "BlogCategory")]
	[RestrictParents(typeof(CategoryContainer))]
	public class Category : BasePage
	{
		
	}
}