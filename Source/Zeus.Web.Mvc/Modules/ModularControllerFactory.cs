using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

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
			var controllerKey = controllerName.ToLowerInvariant() + "controller";

			object area;
			if (requestContext.RouteData.Values.TryGetValue("area", out area))
			{
				var areaControllerKey = Convert.ToString(area).ToLowerInvariant() + "." + controllerKey;
				if (_kernel.HasComponent(areaControllerKey))
				{
					//requestContext.RouteData.Values["controller"] = area + "/" + controllerName;
					return _kernel.Resolve<IController>(areaControllerKey);
				}
			}

			return _kernel.HasComponent(controllerKey) ? _kernel.Resolve<IController>(controllerKey) : null;
		}

		public void ReleaseController(IController controller)
		{
			_kernel.ReleaseComponent(controller);
		}
	}
}