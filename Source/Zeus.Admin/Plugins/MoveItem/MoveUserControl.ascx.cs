using System.Collections.Generic;
using Ext.Net;
using Zeus.Security;
using System;

namespace Zeus.Admin.Plugins.MoveItem
{
	[DirectMethodProxyID(Alias = "Move", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class MoveUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void MoveNode(int source, int destination, int pos)
		{
            int destinationID = Convert.ToInt32(destination);
            if (destinationID < 0)
                destinationID = -1 * (destinationID % 100000);

			ContentItem sourceContentItem = Engine.Persister.Get(source);
            //get abs value of destination - this sorts out placement folders, which have to have the a negative value of their parent node so that sorting, moving etc can work
            ContentItem destinationContentItem = Engine.Persister.Get(destinationID);

			// Check user has permission to create items under the SelectedItem
			if (!Engine.SecurityManager.IsAuthorized(destinationContentItem, Page.User, Operations.Create))
			{
				ExtNet.MessageBox.Alert("Cannot move item", "You are not authorised to move an item to this location.");
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