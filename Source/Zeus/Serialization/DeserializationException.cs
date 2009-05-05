namespace Zeus.Serialization
{
	/// <summary>
	/// An exception that may be thrown when problems occur in the deseralization 
	/// of exported content data.
	/// </summary>
	public class DeserializationException : ZeusException
	{
		public DeserializationException(string message)
			: base(message)
		{
		}
	}
}