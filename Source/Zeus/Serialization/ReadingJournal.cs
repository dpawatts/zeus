using System;
using System.Collections.Generic;

namespace Zeus.Serialization
{
	public class ReadingJournal : IImportRecord
	{
		readonly IList<ContentItem> readItems = new List<ContentItem>();
		readonly IList<Exception> errors = new List<Exception>();
		public event EventHandler<ItemEventArgs> ItemAdded;

		public IList<ContentItem> ReadItems
		{
			get { return readItems; }
		}

		public ContentItem LastItem
		{
			get
			{
				if (readItems.Count == 0)
					return null;
				return readItems[readItems.Count - 1];
			}
		}

		public ContentItem RootItem
		{
			get
			{
				if (readItems.Count == 0)
					return null;
				return readItems[0];
			}
		}

		public IList<Exception> Errors
		{
			get { return errors; }
		}

		public void Report(ContentItem item)
		{
			readItems.Add(item);
			if (ItemAdded != null)
				ItemAdded.Invoke(this, new ItemEventArgs(item));
		}

		public ContentItem Find(int itemiD)
		{
			foreach (ContentItem previousItem in readItems)
				if (previousItem.ID == itemiD)
					return previousItem;
			return null;
		}

		public void Error(Exception ex)
		{
			Errors.Add(ex);
		}
	}
}