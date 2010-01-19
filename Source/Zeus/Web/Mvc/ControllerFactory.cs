using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace Zeus.Web.Mvc
{
	public class ControllerFactory : IControllerFactory
	{
		private readonly IKernel _kernel;

		public ControllerFactory(IKernel kernel)
		{
			_kernel = kernel;
		}

		public IController CreateController(RequestContext requestContext, string controllerName)
		{
			string controllerKey = controllerName.ToLowerInvariant();

			object area;
			if (requestContext.RouteData.Values.TryGetValue("area", out area))
			{
				string areaControllerKey = Convert.ToString(area).ToLowerInvariant() + "." + controllerKey;
				IController areaController = InstantiateController(areaControllerKey);
				if (areaController != null)
					return areaController;
			}

			IController controller = InstantiateController(controllerKey);
			if (controller == null)
				throw new HttpException(404, string.Format(CultureInfo.CurrentUICulture, "The controller for path '{0}' was not found or does not implement IController.", requestContext.HttpContext.Request.Path));
			return controller;
		}

		private IController InstantiateController(string controllerName)
		{
			IController controller = _kernel.TryGet<IController>(controllerName.ToLowerInvariant());

			var standardController = controller as Controller;

			if (standardController != null)
				standardController.ActionInvoker = new NinjectActionInvoker(_kernel);

			return controller;
		}

		public void ReleaseController(IController controller)
		{
			//_kernel.ReleaseComponent(controller);
		}
	}
}