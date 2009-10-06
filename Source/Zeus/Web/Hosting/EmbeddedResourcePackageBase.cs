using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;
using Zeus.Web.Routing;

namespace Zeus.Web.Hosting
{
	public abstract class EmbeddedResourcePackageBase : IEmbeddedResourcePackage
	{
		public abstract void Register(ICollection<RouteBase> routes, ResourceSettings resourceSettings);

		protected void RegisterStandardArea(ICollection<RouteBase> routes, ResourceSettings resourceSettings, string areaName)
		{
			RegisterStandardArea(routes, resourceSettings, areaName, "Content");
		}

		protected void RegisterStandardArea(ICollection<RouteBase> routes, ResourceSettings resourceSettings, string areaName, string clientResourcePrefix)
		{
			Assembly assembly = GetType().Assembly;
			resourceSettings.AssemblyPathPrefixes.Add(areaName, assembly);
			resourceSettings.ClientResourcePrefixes.Add(assembly, clientResourcePrefix);
			RegisterStandardRoutes(routes, assembly, areaName, clientResourcePrefix);
		}

		protected void RegisterStandardRoutes(ICollection<RouteBase> routes, Assembly assembly, string areaName)
		{
			RegisterStandardRoutes(routes, assembly, areaName, "Content");
		}

		protected void RegisterStandardRoutes(ICollection<RouteBase> routes, Assembly assembly, string areaName, string clientResourcePrefix)
		{
			routes.Add(new Route("content/{area}/{*resource}",
				new RouteValueDictionary(),
				new RouteValueDictionary(new { area = areaName }),
				new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + "." + clientResourcePrefix)));
		}
	}
}