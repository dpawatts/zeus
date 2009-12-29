using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Zeus.Web.Hosting;

namespace Zeus.AddIns.Blogs.Admin
{
	public class EmbeddedResourcePackage : EmbeddedResourcePackageBase
	{
		public override void Register(RouteCollection routes, ResourceSettings resourceSettings)
		{
			RegisterStandardArea(routes, resourceSettings, "blogsadmin", "Assets");
			routes.IgnoreRoute("blogsadmin/{*pathInfo}");
		}
	}
}