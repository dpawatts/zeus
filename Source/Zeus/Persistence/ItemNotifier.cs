using System;

namespace Zeus.Persistence
{
	/// <summary>Intercepts content items loads and injects dependencies such as definition manager and url rewriter.</summary>
	public class ItemNotifier : IItemNotifier
	{
		public bool Notify(ContentItem newlyCreatedItem)
		{
			if (newlyCreatedItem != null && ItemCreated != null)
			{
				ItemCreated(this, new ItemEventArgs(newlyCreatedItem));
				return true;
			}
			return false;
		}

		public event EventHandler<ItemEventArgs> ItemCreated;
	}
}
