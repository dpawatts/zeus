using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseCategory")]
	[RestrictParents(typeof(Shop), typeof(Category))]
	public class Category : BasePage
	{
		
	}
}