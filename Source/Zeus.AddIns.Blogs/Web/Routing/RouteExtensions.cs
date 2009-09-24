using System.Collections.Generic;
using System.Web.Routing;
using Zeus.AddIns.Blogs.Web.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.Routing
{
	public static class RouteExtensions
	{
		public static void MapXmlRpcHandler<TXmlRpcHandler>(this ICollection<RouteBase> routes, string url)
			where TXmlRpcHandler : ZeusXmlRpcService
		{
			routes.Add(new Route(url, new XmlRpcRouteHandler<TXmlRpcHandler>()));
		}
	}
}