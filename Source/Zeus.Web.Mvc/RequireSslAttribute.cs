using System.Web;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public class RequireSslAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpRequestBase request = filterContext.HttpContext.Request;
			HttpResponseBase response = filterContext.HttpContext.Response;

			// Check if we're secure or not and if we're on the local box
			if (!request.IsSecureConnection && !request.IsLocal)
			{
				string url = request.Url.ToString().ToLower().Replace("http:", "https:");
				response.Redirect(url);
			}
			base.OnActionExecuting(filterContext);
		}
	}
}