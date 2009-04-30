using System;

namespace Zeus
{
	/// <summary>
	/// Cancellable event argument containing item data.
	/// </summary>
	public class CancelItemEventArgs : ItemEventArgs
	{
		/// <summary>Creates a new instance of the CancellableItemEventArgs.</summary>
		/// <param name="item">The content item to reference with these arguements.</param>
		/// <param name="finalAction"></param>
		public CancelItemEventArgs(ContentItem item, Action<ContentItem> finalAction)
			: base(item)
		{
			FinalAction = finalAction;
		}

		/// <summary>Creates a new instance of the CancellableItemEventArgs.</summary>
		/// <param name="item">The content item to reference with these arguements.</param>
		public CancelItemEventArgs(ContentItem item)
			: base(item)
		{
		}

		/// <summary>Gets or sets whether the event with this argument should be cancelled.</summary>
		public bool Cancel { get; set; }

		/// <summary>The action to execute unless the event is cancelled. This action can be exchanged by observers to alter the default behaviour.</summary>
		public Action<ContentItem> FinalAction { get; set; }
	}
}