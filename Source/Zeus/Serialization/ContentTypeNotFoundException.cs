using System.Collections.Generic;

namespace Zeus.Serialization
{
	public class ContentTypeNotFoundException : DeserializationException
	{
		readonly Dictionary<string, string> attributes;

		public ContentTypeNotFoundException(string message, Dictionary<string, string> attributes)
			: base(message)
		{
			this.attributes = attributes;
		}

		public Dictionary<string, string> Attributes
		{
			get { return attributes; }
		}
	}
}