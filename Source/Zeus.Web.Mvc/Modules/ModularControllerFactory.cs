using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace Zeus.Web.Mvc.Modules
{
	public class ModularControllerFactory : IControllerFactory
	{
		private readonly IKernel _kernel;

		public ModularControllerFactory(IKernel kernel)
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

			return InstantiateController(controllerKey);
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