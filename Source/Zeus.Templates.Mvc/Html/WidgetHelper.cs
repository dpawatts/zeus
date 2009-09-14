using System.Collections.Generic;
using Zeus.Web.Mvc.Html;
using Zeus.Web.UI;

namespace Zeus.Templates.Mvc.Html
{
	public class WidgetHelper : BaseWidgetHelper
	{
		public WidgetHelper(IContentItemContainer container, string actionName, string[] zoneNames)
			: base(container, actionName)
		{
			ZoneNames = zoneNames;
		}

		public WidgetHelper(IContentItemContainer container, ContentItem item, string actionName, string[] zoneNames)
			: base(container, item, actionName)
		{
			ZoneNames = zoneNames;
		}

		protected string[] ZoneNames { get; set; }

		protected override IEnumerable<ContentItem> GetItems()
		{
			if (PartsAdapter == null)
				return CurrentItem.GetChildren(ZoneNames);

			return PartsAdapter.GetItemsInZones(CurrentItem, ZoneNames);
		}
	}
}