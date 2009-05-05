namespace Zeus.Serialization
{
	public class WrongVersionException : DeserializationException
	{
		public WrongVersionException(string message)
			: base(message)
		{
		}
	}
}