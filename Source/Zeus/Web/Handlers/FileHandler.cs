using System;
using System.Web;
using Zeus.Admin;
using Zeus.FileSystem;

namespace Zeus.Web.Handlers
{
	public class FileHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			File file = Context.Current.Resolve<Navigator>().Navigate(context.Request.QueryString["Path"]) as File;
			if (file == null)
				return;

			context.Response.ContentType = file.ContentType;
			context.Response.OutputStream.Write(file.Data, 0, file.Data.Length);
			context.Response.Flush();
		}
	}
}
