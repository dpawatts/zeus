using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class TemplateRenderer : ITemplateRenderer
	{
		private readonly IControllerMapper _controllerMapper;

		public TemplateRenderer(IControllerMapper controllerMapper)
		{
			_controllerMapper = controllerMapper;
		}

		public string RenderTemplate(HtmlHelper htmlHelper, ContentItem item, IContentItemContainer container, string action)
		{
			RouteValueDictionary routeValues = new RouteValueDictionary();
			routeValues.Add(ContentRoute.ContentItemKey, item);

			return htmlHelper.Action(action,
				_controllerMapper.GetControllerName(item.GetType()),
				routeValues).ToString();
		}
	}
}