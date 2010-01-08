using System.Web;
using System.Web.Routing;
using Zeus.Templates.Services;

namespace Zeus.Templates.Web.Routing
{
	public class BBCodeRouteHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new BBCodeHttpHandler();
		}

		public class BBCodeHttpHandler : IHttpHandler
		{
			public void ProcessRequest(HttpContext context)
			{
				string result = string.Empty;
				if (!string.IsNullOrEmpty(context.Request.Form["bbCode"]))
					result = Context.Current.Resolve<BBCodeService>().GetHtml(context.Request.Form["bbCode"]);
				context.Response.Write(result);
			}

			public bool IsReusable
			{
				get { return true; }
			}
		}
	}
}