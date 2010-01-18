using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class ZoneHelper : BaseWidgetHelper
	{
		public ZoneHelper(HtmlHelper htmlHelper, IContentItemContainer container, string actionName, string zoneName)
			: base(htmlHelper, container, actionName)
		{
			ZoneName = zoneName;
		}

		public ZoneHelper(HtmlHelper htmlHelper, IContentItemContainer container, ContentItem item, string actionName, string zoneName)
			: base(htmlHelper, container, item, actionName)
		{
			ZoneName = zoneName;
		}

		protected string ZoneName { get; set; }

		protected override IEnumerable<WidgetContentItem> GetItems()
		{
			if (PartsAdapter == null)
				return Engine.ContentManager.GetWidgets(CurrentItem, ZoneName);

			return PartsAdapter.GetItemsInZones(CurrentItem, ZoneName);
		}
	}
}