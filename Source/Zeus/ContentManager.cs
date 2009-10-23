using System.Collections.Generic;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web;

namespace Zeus
{
	public class ContentManager : IContentManager
	{
		/// <summary>Gets children the current user is allowed to access belonging to a certain zone,
		/// i.e. get only children with a certain zone name. </summary>
		/// <param name="item"></param>
		/// <param name="zoneName">The name of the zone.</param>
		/// <returns>A list of items that have the specified zone name.</returns>
		/// <remarks>This method is used by Zeus when when non-page items are added to a zone on a page 
		/// and in edit mode when displaying which items are placed in a certain zone. 
		/// Keep this in mind when overriding this method.</remarks>
		public virtual IEnumerable<WidgetContentItem> GetWidgets(ContentItem item, string zoneName)
		{
			return GetWidgets(item).InZone(zoneName);
		}

		public virtual IEnumerable<WidgetContentItem> GetWidgets(ContentItem item, params string[] zoneNames)
		{
			return GetWidgets(item).InZones(zoneNames);
		}

		public virtual IEnumerable<WidgetContentItem> GetAllWidgets(ContentItem item)
		{
			return GetWidgets(item).InZones();
		}

		private static IEnumerable<WidgetContentItem> GetWidgets(ContentItem item)
		{
			return item.GetChildren<WidgetContentItem>().Authorized(Operations.Read);
		}
	}
}