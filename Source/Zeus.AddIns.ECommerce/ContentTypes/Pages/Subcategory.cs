using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType]
	[RestrictParents(typeof(Category))]
	public class Subcategory : BasePage
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Subcategory), "Zeus.AddIns.ECommerce.Icons.page_red.png"); }
		}
	}
}