using System;
using Zeus.ContentTypes;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// Event argument containing item data.
	/// </summary>
	public class ItemViewEditableObjectEventArgs : EventArgs
	{
		/// <summary>Creates a new instance of the ItemEventArgs.</summary>
		/// <param name="item">The item the associated with these event arguments.</param>
		public ItemViewEditableObjectEventArgs(IEditableObject item)
		{
			AffectedItem = item;
		}

		/// <summary>Gets or sets the item associated with these arguments.</summary>
		public IEditableObject AffectedItem { get; set; }
	}
}