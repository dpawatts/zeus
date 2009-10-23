using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Zeus.Web.Mvc.Modules;

namespace Zeus.AddIns.ECommerce.Mvc
{
	public class ECommerceWebPackage : WebPackageBase
	{
		public const string AREA_NAME = "ECommerce";

		public override void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines)
		{
			RegisterStandardArea(container, routes, viewEngines, AREA_NAME);
		}
	}
}