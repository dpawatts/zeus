using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;
using Zeus.Web.Hosting;

namespace Zeus.AddIns.Blogs.Admin
{
	public class EmbeddedResourcePackage : EmbeddedResourcePackageBase
	{
		public override void Register(ICollection<RouteBase> routes, ResourceSettings resourceSettings)
		{
			RegisterStandardArea(routes, resourceSettings, "blogs", "Assets");
		}
	}
}