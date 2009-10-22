using System.Collections.Generic;
using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.Plugins.MoveItem
{
	[AjaxMethodProxyID(Alias = "Move", IDMode = AjaxMethodProxyIDMode.Alias)]
	public partial class MoveUserControl : PluginUserControlBase
	{
		[AjaxMethod]
		public void MoveNode(int source, int destination, int pos)
		{
			ContentItem sourceContentItem = Engine.Persister.Get(source);
			ContentItem destinationContentItem = Engine.Persister.Get(destination);

			// Check user has permission to create items under the SelectedItem
			if (!Engine.SecurityManager.IsAuthorized(destinationContentItem, Page.User, Operations.Create))
			{
				Ext.MessageBox.Alert("Cannot move item", "You are not authorised to move an item to this location.");
				return;
			}

			// Change parent if necessary.
			if (sourceContentItem.Parent.ID != destinationContentItem.ID)
				Zeus.Context.Persister.Move(sourceContentItem, destinationContentItem);

			// Update sort order based on new pos.
			IList<ContentItem> siblings = sourceContentItem.Parent.Children;
			Utility.MoveToIndex(siblings, sourceContentItem, pos);
			foreach (ContentItem updatedItem in Utility.UpdateSortOrder(siblings))
				Zeus.Context.Persister.Save(updatedItem);
		}
	}
}