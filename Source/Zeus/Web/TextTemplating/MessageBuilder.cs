using System.IO;
using System.Reflection;

namespace Zeus.Web.TextTemplating
{
	public abstract class MessageBuilder : IMessageBuilder
	{
		public abstract void Initialize(Assembly templateAssembly, string templateResourcePath);

		public string Transform(string message, object data)
		{
			var writer = new StringWriter();
			Transform(message, data, writer);
			return writer.ToString();
		}

		protected abstract void Transform(string message, object data, TextWriter output);
	}
}