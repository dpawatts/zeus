using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Ninject;
using System.Linq;

namespace Zeus.Web.Mvc
{
	public class ControllerFactory : DefaultControllerFactory
	{
		private readonly IKernel _kernel;

		public ControllerFactory(IKernel kernel)
		{
			_kernel = kernel;
		}

		public override IController CreateController(RequestContext requestContext, string controllerName)
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

			if (controller != null)
                return controller;

            return base.CreateController(requestContext, controllerName);
		}

		public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
		{
			return SessionStateBehavior.Default;
		}

		private IController InstantiateController(string controllerName)
		{
			IController controller = _kernel.TryGet<IController>(controllerName.ToLowerInvariant());

			var standardController = controller as Controller;

			if (standardController != null)
				standardController.ActionInvoker = new NinjectActionInvoker(_kernel);

			return controller;
		}
	}
}