using System.Collections.Generic;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class ZoneHelper : BaseWidgetHelper
	{
		public ZoneHelper(IContentItemContainer container, string actionName, string zoneName)
			: base(container, actionName)
		{
			ZoneName = zoneName;
		}

		public ZoneHelper(IContentItemContainer container, ContentItem item, string actionName, string zoneName)
			: base(container, item, actionName)
		{
			ZoneName = zoneName;
		}

		protected string ZoneName { get; set; }

		protected override IEnumerable<ContentItem> GetItems()
		{
			if (PartsAdapter == null)
				return CurrentItem.GetChildren(ZoneName);

			return PartsAdapter.GetItemsInZones(CurrentItem, ZoneName);
		}
	}
}