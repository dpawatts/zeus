using System;
using System.Web.UI;

namespace Zeus.ContentTypes.Properties
{
	public interface IDisplayer : IUniquelyNamed
	{
		Control AddTo(Control container, ContentItem item, string propertyName);
	}
}
