using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Zeus.Web.Mvc.Modules;

namespace Zeus.AddIns.Forums.Mvc
{
	public class ForumsWebPackage : WebPackageBase
	{
		public const string AREA_NAME = "Forums";

		public override void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines)
		{
			RegisterStandardArea(container, routes, viewEngines, AREA_NAME);
		}
	}
}