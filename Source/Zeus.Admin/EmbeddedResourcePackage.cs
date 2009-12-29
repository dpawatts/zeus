using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;
using Zeus.Web.Hosting;

namespace Zeus.Admin
{
	public class ZeusAdminEmbeddedResourcePackage : EmbeddedResourcePackageBase
	{
		public override void Register(RouteCollection routes, ResourceSettings resourceSettings)
		{
			RegisterStandardArea(routes, resourceSettings, "admin", "Assets");
		}
	}
}