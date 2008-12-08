using System;
using System.Web.UI;

namespace Zeus.ContentTypes.Properties
{
	public interface IDisplayer
	{
		Control AddTo(Control container, ContentItem item, string propertyName);
	}
}
