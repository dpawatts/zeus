using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class DisplayHelper : ItemHelper
	{
		private readonly ITemplateRenderer _templateRenderer = Context.Current.Resolve<ITemplateRenderer>();

		public DisplayHelper(IContentItemContainer container, ContentItem item)
			: base(container, item)
		{
			
		}

		public override string ToString()
		{
			return _templateRenderer.RenderTemplate(CurrentItem, Container, "Index");
		}
	}
}