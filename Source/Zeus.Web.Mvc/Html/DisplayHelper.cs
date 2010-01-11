using System.Web.Mvc;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class DisplayHelper : ItemHelper
	{
		private readonly ITemplateRenderer _templateRenderer = Context.Current.Resolve<ITemplateRenderer>();

		public DisplayHelper(ViewContext viewContext, IContentItemContainer container, ContentItem item)
			: base(viewContext, container, item)
		{
			
		}

		public override string ToString()
		{
			return _templateRenderer.RenderTemplate(ViewContext, CurrentItem, Container, "Index");
		}
	}
}