using Ext.Net;
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
			get { return Utility.GetCooliteIconUrl(Icon.PageRed); }
		}
	}
}