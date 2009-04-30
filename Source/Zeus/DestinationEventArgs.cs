namespace Zeus
{
	/// <summary>
	/// Event argument containing item and destination item.
	/// </summary>
	public class DestinationEventArgs : ItemEventArgs
	{
		/// <summary>Creates a new instance of the DestinationEventArgs.</summary>
		/// <param name="affectedItem">The item associated with these arguments.</param>
		/// <param name="destination">The destination for the event with these arguments.</param>
		public DestinationEventArgs(ContentItem affectedItem, ContentItem destination)
			: base(affectedItem)
		{
			Destination = destination;
		}

		/// <summary>Gets the destination for the event with these arguments.</summary>
		public ContentItem Destination { get; private set; }
	}
}