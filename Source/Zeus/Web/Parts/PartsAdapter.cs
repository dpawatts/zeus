using System.Collections.Generic;
using System.Security.Principal;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Engine;
using Zeus.Web.UI;

namespace Zeus.Web.Parts
{
	/// <summary>
	/// Controls aspects related to zones, zone definitions, and items to display in a zone.
	/// </summary>
	[Controls(typeof(ContentItem))]
	public class PartsAdapter : AbstractContentAdapter
	{
		/// <summary>Retrieves content items added to a zone of the parnet item.</summary>
		/// <param name="parentItem">The item whose items to get.</param>
		/// <param name="zoneName">The zone in which the items should be contained.</param>
		/// <returns>A list of items in the zone.</returns>
		public virtual IEnumerable<ContentItem> GetItemsInZone(ContentItem parentItem, string zoneName)
		{
			if (parentItem == null)
				return new List<ContentItem>();

			return parentItem.GetChildren(zoneName);
		}

		/// <summary>Retrieves allowed item definitions.</summary>
		/// <param name="parentItem">The parent item.</param>
		/// <param name="zoneName">The zone where children would be placed.</param>
		/// <param name="user">The user to restrict access for.</param>
		/// <returns>Item definitions allowed by zone, parent restrictions and security.</returns>
		public virtual IEnumerable<ContentType> GetAllowedDefinitions(ContentItem parentItem, string zoneName, IPrincipal user)
		{
			ContentType containerDefinition = Engine.ContentTypes.GetContentType(parentItem.GetType());

			foreach (ContentType childDefinition in containerDefinition.AllowedChildren)
				if (childDefinition.IsAllowedInZone(zoneName) && childDefinition.Enabled && childDefinition.IsAuthorized(user))
					yield return childDefinition;
		}

		/// <summary>Gets the path to the given item's template. This is a way to override the default template provided by the content item.</summary>
		/// <param name="item">The item whose path is requested.</param>
		/// <returns>The virtual path of the template or null if the item is not supposed to be added.</returns>
		protected virtual string GetTemplateUrl(ContentItem item)
		{
			return item.FindPath(PathData.DefaultAction).TemplateUrl;
		}
	}
}