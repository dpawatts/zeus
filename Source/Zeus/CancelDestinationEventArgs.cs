using System;

namespace Zeus
{
	/// <summary>
	/// Event argument containing item and destination item.
	/// </summary>
	public class CancelDestinationEventArgs : DestinationEventArgs
	{
		public CancelDestinationEventArgs(ContentItem item, ContentItem destination, Func<ContentItem, ContentItem, ContentItem> finalAction)
			: base(item, destination)
		{
			FinalAction = finalAction;
		}

		public CancelDestinationEventArgs(ContentItem item, ContentItem destination)
			: base(item, destination)
		{
		}

		/// <summary>Gets or sets whether the event with this argument should be cancelled.</summary>
		public bool Cancel { get; set; }

		/// <summary>The action to execute unless the event is cancelled. This action can be exchanged by observers to alter the default behaviour.</summary>
		public Func<ContentItem, ContentItem, ContentItem> FinalAction { get; set; }
	}
}