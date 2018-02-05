using System.Web.Mvc;
using System.Web.Routing;
using Zeus.Web.Hosting;

namespace Zeus.AddIns.ECommerce.Admin
{
	public class EmbeddedResourcePackage : EmbeddedResourcePackageBase
	{
		public override void Register(RouteCollection routes, ResourceSettings resourceSettings)
		{
			RegisterStandardArea(routes, resourceSettings, "ecommerceadmin", "Assets");
			routes.IgnoreRoute("ecommerceadmin/{*pathInfo}");
		}
	}
}