namespace Zeus.Integrity
{
	/// <summary>
	/// Exception thrown when an attempt to move an item onto or below itself is made.
	/// </summary>
	public class DestinationOnOrBelowItselfException : ZeusException
	{
		public DestinationOnOrBelowItselfException(ContentItem source, ContentItem destination)
			: base("Cannot move item to a destination onto or below itself.")
		{
			SourceItem = source;
			DestinationItem = destination;
		}

		/// <summary>Gets the source item that is causing the conflict.</summary>
		public ContentItem SourceItem
		{
			get;
			private set;
		}

		/// <summary>Gets the parent item already containing an item with the same name.</summary>
		public ContentItem DestinationItem
		{
			get;
			private set;
		}
	}
}