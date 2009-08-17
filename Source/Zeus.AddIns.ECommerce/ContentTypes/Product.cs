using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(Shop))]
	public class Product : BasePage
	{
		[LinkedItemsCheckBoxListEditor("Groups", 100, typeof(GroupItem))]
		public PropertyCollection GroupItems
		{
			get { return GetDetailCollection("GroupItems", true); }
		}
	}
}