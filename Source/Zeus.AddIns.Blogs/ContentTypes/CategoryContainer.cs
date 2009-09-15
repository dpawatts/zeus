using System.Collections.Generic;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Category Container")]
	[RestrictParents(typeof(Blog))]
	public class CategoryContainer : BasePage
	{
		
	}
}