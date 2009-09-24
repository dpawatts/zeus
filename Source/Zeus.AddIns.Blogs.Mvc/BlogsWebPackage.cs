using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Zeus.AddIns.Blogs.Web.MetaWeblog;
using Zeus.AddIns.Blogs.Web.Pingback;
using Zeus.AddIns.Blogs.Web.Routing;
using Zeus.Web.Mvc.Modules;

namespace Zeus.AddIns.Blogs.Mvc
{
	public class BlogsWebPackage : WebPackageBase
	{
		public const string AREA_NAME = "Blogs";

		public override void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines)
		{
			routes.MapXmlRpcHandler<MetaWeblogXmlRpcService>("services/metaweblog");
			routes.MapXmlRpcHandler<PingbackXmlRpcService>("services/pingbacks");
			RegisterStandardArea(container, routes, viewEngines, AREA_NAME);
		}
	}
}