using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.Admin
{
	public class TreeSorter : ITreeSorter
	{
		private readonly IPersister persister;
		private readonly IAdminManager editManager;
		private readonly IWebContext webContext;

		public TreeSorter(IPersister persister, IAdminManager editManager, IWebContext webContext)
		{
			this.persister = persister;
			this.editManager = editManager;
			this.webContext = webContext;
		}

		#region ITreeSorter Members

		public void MoveUp(ContentItem item)
		{
			if (item.Parent != null)
			{
				var filter = editManager.GetEditorFilter(webContext.User);
				IList<ContentItem> siblings = item.Parent.Children;
				IList<ContentItem> filtered = filter(siblings).ToList();

				int index = filtered.IndexOf(item);
				if (index > 0)
					MoveTo(item, NodePosition.Before, filtered[index - 1]);
			}
		}

		public void MoveDown(ContentItem item)
		{
			if (item.Parent != null)
			{
				var filter = editManager.GetEditorFilter(webContext.User);
				IList<ContentItem> siblings = item.Parent.Children;
				IList<ContentItem> filtered = filter(siblings).ToList();

				int index = filtered.IndexOf(item);
				if (index + 1 < filtered.Count)
					MoveTo(item, NodePosition.After, filtered[index + 1]);
			}
		}

		public void MoveTo(ContentItem item, int index)
		{
			IList<ContentItem> siblings = item.Parent.Children;
			Utility.MoveToIndex(siblings, item, index);
			foreach (ContentItem updatedItem in Utility.UpdateSortOrder(siblings))
			{
				persister.Save(updatedItem);
			}
		}

		public void MoveTo(ContentItem item, NodePosition position, ContentItem relativeTo)
		{
			if (relativeTo == null) throw new ArgumentNullException("item");
			if (relativeTo == null) throw new ArgumentNullException("relativeTo");
			if (relativeTo.Parent == null) throw new ArgumentException("The supplied item '" + relativeTo + "' has no parent to add to.", "relativeTo");

			if (item.Parent != relativeTo.Parent)
				item.AddTo(relativeTo.Parent);

			IList<ContentItem> siblings = item.Parent.Children;

			int itemIndex = siblings.IndexOf(item);
			int relativeToIndex = siblings.IndexOf(relativeTo);

			if (itemIndex < 0)
			{
				if (position == NodePosition.Before)
					siblings.Insert(relativeToIndex, item);
				else
					siblings.Insert(relativeToIndex + 1, item);
			}
			else if (itemIndex < relativeToIndex && position == NodePosition.Before)
				MoveTo(item, relativeToIndex - 1);
			else if (itemIndex > relativeToIndex && position == NodePosition.After)
				MoveTo(item, relativeToIndex + 1);
			else
				MoveTo(item, relativeToIndex);
		}

		#endregion
	}
}
