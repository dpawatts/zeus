using System;

namespace Zeus
{
	/// <summary>
	/// Event argument containing item data.
	/// </summary>
	public class ItemEventArgs : EventArgs
	{
		/// <summary>Creates a new instance of the ItemEventArgs.</summary>
		/// <param name="item">The item the associated with these event arguments.</param>
		public ItemEventArgs(ContentItem item)
		{
			this.AffectedItem = item;
		}

		/// <summary>Gets or sets the item associated with these arguments.</summary>
		public ContentItem AffectedItem
		{
			get;
			set;
		}
	}
}
