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
            try
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
                    //ExtNet.MessageBox.Alert("Cannot move item", "You are not authorised to move an item to this location.");
                    //Ext.Net.ResourceManager.AjaxSuccess = false;
                    //Ext.Net.ResourceManager.AjaxErrorMessage = "You are not authorised to move an item to this location.";
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

                //set the updated value on the parent of the item that has been moved (for caching purposes)
                sourceContentItem.Parent.Updated = Utility.CurrentTime();
                Zeus.Context.Persister.Save(sourceContentItem.Parent);

                ContentItem theParent = sourceContentItem.Parent.Parent;
                while (theParent.Parent != null)
                {
                    //go up the tree updating - if a child has been changed, so effectively has the parent
                    theParent.Updated = DateTime.Now;
                    Zeus.Context.Persister.Save(theParent);
                    theParent = theParent.Parent;
                }
            }
            catch (Exception ex)
            {
                //ExtNet.MessageBox.Alert("Cannot move item", ex.Message + " : " + ex.StackTrace);				
            }
		}
	}
}