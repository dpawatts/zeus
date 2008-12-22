using System;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		/// <summary>Occurs when an item has been deleted</summary>
		event EventHandler<ItemEventArgs> ItemDeleted;
		/// <summary>Occurs when an item has been saved</summary>
		event EventHandler<ItemEventArgs> ItemSaved;

		ContentItem Copy(ContentItem source, ContentItem destination);
		ContentItem Copy(ContentItem source, ContentItem destination, bool includeChildren);
		void Delete(ContentItem contentItem);
		ContentItem Get(int id);
		T Get<T>(int id) where T : ContentItem;
		ContentItem Load(int id);
		void Move(ContentItem toMove, ContentItem newParent);
		void Save(ContentItem contentItem);
		void UpdateSortOrder(ContentItem contentItem, int newPos);
	}
}
