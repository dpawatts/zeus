using System.Collections.Generic;
using System.Web.Routing;

namespace Zeus.Web.Hosting
{
	public interface IEmbeddedResourcePackage
	{
		void Register(RouteCollection routes, ResourceSettings resourceSettings);
	}
}