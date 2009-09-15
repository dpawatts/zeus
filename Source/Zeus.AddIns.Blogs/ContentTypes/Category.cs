using System.Collections.Generic;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(CategoryContainer))]
	public class Category : BasePage
	{
		
	}
}