using System.Web;
using System.Web.Routing;
using Zeus.AddIns.Blogs.Web.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.Routing
{
	public class XmlRpcRouteHandler<THandler> : IRouteHandler
		where THandler : ZeusXmlRpcService
	{
		#region IRouteHandler Members

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return Context.Current.Resolve<THandler>();
		}

		#endregion
	}
}