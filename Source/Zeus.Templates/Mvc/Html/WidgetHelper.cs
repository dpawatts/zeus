using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.Web;
using Zeus.Web.Mvc.Html;
using Zeus.Web.UI;

namespace Zeus.Templates.Mvc.Html
{
	public class WidgetHelper : BaseWidgetHelper
	{
		public WidgetHelper(ViewContext viewContext, IContentItemContainer container, string actionName, string[] zoneNames)
			: base(viewContext, container, actionName)
		{
			ZoneNames = zoneNames;
		}

		public WidgetHelper(ViewContext viewContext, IContentItemContainer container, ContentItem item, string actionName, string[] zoneNames)
			: base(viewContext, container, item, actionName)
		{
			ZoneNames = zoneNames;
		}

		protected string[] ZoneNames { get; set; }

		protected override IEnumerable<WidgetContentItem> GetItems()
		{
			if (PartsAdapter == null)
				return Engine.ContentManager.GetWidgets(CurrentItem, ZoneNames);

			return PartsAdapter.GetItemsInZones(CurrentItem, ZoneNames);
		}
	}
}