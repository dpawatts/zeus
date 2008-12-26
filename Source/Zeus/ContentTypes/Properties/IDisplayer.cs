using System;
using System.Web.UI;

namespace Zeus.ContentTypes.Properties
{
	public interface IDisplayer : IUniquelyNamed, ITemplate
	{
		string Title { get; }
		void SetValue(Control container, ContentItem item, string propertyName);
	}
}
