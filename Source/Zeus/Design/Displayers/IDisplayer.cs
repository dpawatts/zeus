using System.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Design.Displayers
{
	public interface IDisplayer : IUniquelyNamed, ITemplate
	{
		string Title { get; }
		void SetValue(Control container, ContentItem item, string propertyName);
	}
}