using System.Collections.Generic;
using System.Linq;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.Parts;

namespace Zeus.Templates.Services
{
	/// <summary>
	/// Implements "Recursive" zones functionality.
	/// </summary>
	[Controls(typeof(BasePage))]
	public class TemplatesPartsAdapter : PartsAdapter
	{
		public override IEnumerable<WidgetContentItem> GetItemsInZones(ContentItem parentItem, params string[] zoneNames)
		{
			List<WidgetContentItem> items = base.GetItemsInZones(parentItem, zoneNames).ToList();
			ContentItem grandParentItem = parentItem;
			foreach (string zoneName in zoneNames)
				if (zoneName.StartsWith("Recursive") && grandParentItem is BasePage)
				{
					if (parentItem.VersionOf == null)
						items.AddRange(GetItemsInZones(parentItem.Parent, zoneName));
					else
						items.AddRange(GetItemsInZones(parentItem.VersionOf.Parent, zoneName));
				}
			return items;
		}
	}
}