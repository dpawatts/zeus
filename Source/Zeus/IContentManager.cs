using System.Collections.Generic;
using Zeus.Web;

namespace Zeus
{
	public interface IContentManager
	{
		/// <summary>Gets children the current user is allowed to access belonging to a certain zone,
		/// i.e. get only children with a certain zone name. </summary>
		/// <param name="item"></param>
		/// <param name="zoneName">The name of the zone.</param>
		/// <returns>A list of items that have the specified zone name.</returns>
		/// <remarks>This method is used by Zeus when when non-page items are added to a zone on a page 
		/// and in edit mode when displaying which items are placed in a certain zone. 
		/// Keep this in mind when overriding this method.</remarks>
		IEnumerable<WidgetContentItem> GetWidgets(ContentItem item, string zoneName);

		IEnumerable<WidgetContentItem> GetWidgets(ContentItem item, params string[] zoneNames);
		IEnumerable<WidgetContentItem> GetAllWidgets(ContentItem item);
	}
}