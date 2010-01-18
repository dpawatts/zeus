using System.Web;
using System.Web.Routing;

namespace Zeus.BaseLibrary.Web.Routing
{
	public class DebugRouteHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			DebugHttpHandler handler = new DebugHttpHandler();
			handler.RequestContext = requestContext;
			return handler;
		}
	}
}