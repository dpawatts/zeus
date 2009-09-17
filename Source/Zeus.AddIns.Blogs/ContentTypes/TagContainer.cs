using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Tag Container")]
	[RestrictParents(typeof(Blog))]
	public class TagContainer : BasePage
	{
		
	}
}