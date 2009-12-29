using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;
using Zeus.Web.Hosting;

namespace Zeus
{
	public class ZeusEmbeddedResourcePackage : EmbeddedResourcePackageBase
	{
		public override void Register(RouteCollection routes, ResourceSettings resourceSettings)
		{
			RegisterStandardArea(routes, resourceSettings, "zeus", "Web.Resources");
		}
	}
}