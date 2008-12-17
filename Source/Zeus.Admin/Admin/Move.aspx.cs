using System;
using Isis.Web;

namespace Zeus.Admin
{
	public partial class Move : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem sourceContentItem = this.SelectedItem;
			ContentItem destinationContentItem = Zeus.Context.Current.Resolve<Navigator>().Navigate(Request.GetRequiredString("destination"));

			// Change parent if necessary.
			if (sourceContentItem.Parent.ID != destinationContentItem.ID)
				Zeus.Context.Persister.Move(sourceContentItem, destinationContentItem);

			// Update sort order based on new pos.
			int pos = Request.GetRequiredInt("pos");
			Zeus.Context.Persister.UpdateSortOrder(sourceContentItem, pos);

			Refresh(sourceContentItem, AdminFrame.Both, false);
		}
	}
}
