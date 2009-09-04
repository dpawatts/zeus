using System.Collections.Generic;
using System.Linq;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.Parts;

namespace Zeus.Templates.Services
{
	/// <summary>
	/// Implements "Recusive" zones functionality.
	/// </summary>
	[Controls(typeof(BasePage))]
	public class TemplatesPartsAdapter : PartsAdapter
	{
		public override IEnumerable<ContentItem> GetItemsInZone(ContentItem parentItem, string zoneName)
		{
			List<ContentItem> items = base.GetItemsInZone(parentItem, zoneName).ToList();
			ContentItem grandParentItem = parentItem;
			if (zoneName.StartsWith("Recursive") && grandParentItem is BasePage)
			{
				if (parentItem.VersionOf == null)
					items.AddRange(GetItemsInZone(parentItem.Parent, zoneName));
				else
					items.AddRange(GetItemsInZone(parentItem.VersionOf.Parent, zoneName));
			}
			return items;
		}
	}
}