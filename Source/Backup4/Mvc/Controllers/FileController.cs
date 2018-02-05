using System.Web.Mvc;
using Zeus.FileSystem;
using Zeus.Web;
using Zeus.FileSystem.Images;
using System;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(File), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class FileController : ZeusController<File>
	{
		public override ActionResult Index()
		{
            if (CurrentItem is Image)
            {
                System.Web.HttpContext httpContext = System.Web.HttpContext.Current;
                httpContext.Response.Headers["Cache-Control"] = "public";
                DateTime modDate = CurrentItem.Updated;
                
                httpContext.Response.Headers["Last-Modified"] = modDate.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");

                if (CurrentItem.Size.HasValue)
                    httpContext.Response.Headers["Content-Length"] = CurrentItem.Size.Value.ToString();

                httpContext.Response.Headers["Content-Type"] = CurrentItem.ContentType;
                DateTime dateOnRequest;
                if (DateTime.TryParse(httpContext.Request.Headers["If-Modified-Since"], out dateOnRequest) && (dateOnRequest == modDate))
                {
                    httpContext.Response.StatusCode = 304;
                    httpContext.Response.StatusDescription = "Not Modified";
                    httpContext.Response.SuppressContent = true;
                    httpContext.Response.Flush();
                    httpContext.Response.End();
                    return null;
                }
            }

			return new FileContentResult(CurrentItem.Data, CurrentItem.ContentType);

		}
	}
}