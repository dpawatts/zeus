using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(TagContainer))]
	public class Tag : BasePage
	{
		
	}
}