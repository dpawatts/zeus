using System.Web.Mvc;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class DisplayHelper : ItemHelper
	{
		private readonly ITemplateRenderer _templateRenderer = Context.Current.Resolve<ITemplateRenderer>();

		public DisplayHelper(HtmlHelper htmlHelper, IContentItemContainer container, ContentItem item)
			: base(htmlHelper, container, item)
		{
			
		}

		public override string ToString()
		{
			return _templateRenderer.RenderTemplate(HtmlHelper, CurrentItem, Container, "Index");
		}
	}
}