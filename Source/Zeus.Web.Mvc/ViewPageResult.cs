using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Zeus.Web.Mvc
{
	/// <summary>
	/// A custom <see cref="ActionResult"/> that invokes the MVC pipeline again for the given page.
	/// </summary>
	public class ViewPageResult : ActionResult
	{
		private readonly ContentItem _thePage;
		private readonly IControllerMapper _controllerMapper;
		private readonly IWebContext _webContext;
		private readonly IActionInvoker _actionInvoker;

		public ViewPageResult(ContentItem thePage, IControllerMapper controllerMapper, IWebContext webContext, IActionInvoker actionInvoker)
		{
			_thePage = thePage;
			_controllerMapper = controllerMapper;
			_webContext = webContext;
			_actionInvoker = actionInvoker;
		}

		/// <summary>
		/// Enables processing of the result of an action method by a custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
		/// </summary>
		/// <param name="context"/>
		public override void ExecuteResult(ControllerContext context)
		{
			SetupZeusForNewPageRequest();

			ControllerBase controller = BuildController(context);

			_actionInvoker.InvokeAction(controller.ControllerContext, "Index");
		}

		private void SetupZeusForNewPageRequest()
		{
			_webContext.CurrentPage = _thePage;
		}

		private ControllerBase BuildController(ControllerContext context)
		{
			var routeData = context.RouteData;
			routeData.Values[ContentRoute.ContentItemKey] = _thePage;
			routeData.Values[ContentRoute.ContentItemIdKey] = _thePage.ID;

			var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);

			var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory()
				.CreateController(requestContext, _controllerMapper.GetControllerName(_thePage.GetType()));

			controller.ControllerContext = new ControllerContext(requestContext, controller);

			return controller;
		}
	}
}