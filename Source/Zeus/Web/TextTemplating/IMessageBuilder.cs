using System.Reflection;

namespace Zeus.Web.TextTemplating
{
	public interface IMessageBuilder
	{
		void Initialize(Assembly templateAssembly, string templateResourcePath);
		string Transform(string message, object data);
	}
}