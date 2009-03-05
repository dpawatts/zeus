using System.Collections.Generic;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.Admin
{
	public class TreeSorter : ITreeSorter
	{
		private readonly IPersister persister;

		public TreeSorter(IPersister persister)
		{
			this.persister = persister;
		}

		#region ITreeSorter Members

		public void MoveTo(ContentItem item, int index)
		{
			IList<ContentItem> siblings = item.Parent.Children;
			Utility.MoveToIndex(siblings, item, index);
			foreach (ContentItem updatedItem in Utility.UpdateSortOrder(siblings))
			{
				persister.Save(updatedItem);
			}
		}

		#endregion
	}
}