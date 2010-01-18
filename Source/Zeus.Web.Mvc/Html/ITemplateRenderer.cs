using System.Web.Mvc;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public interface ITemplateRenderer
	{
		string RenderTemplate(HtmlHelper htmlHelper, ContentItem item, IContentItemContainer container, string action);
	}
}