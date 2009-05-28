using System;
using System.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Design.Displayers
{
	public interface IDisplayer : IUniquelyNamed, ITemplate, IComparable<IDisplayer>
	{
		string Title { get; }
		void SetValue(Control container, ContentItem item, string propertyName);
	}
}