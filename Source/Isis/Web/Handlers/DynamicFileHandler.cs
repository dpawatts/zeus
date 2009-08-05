using System;
using System.Web;
using Isis.ExtensionMethods;
using Isis.Web.UI.WebControls;

namespace Isis.Web.Handlers
{
	public class DynamicFileHandler : IHttpHandler
	{
		#region Properties

		public bool IsReusable
		{
			get { return false; }
		}

		#endregion

		#region Methods

		public void ProcessRequest(HttpContext context)
		{
			DatabaseSource source = context.Request.QueryString["file"].Replace(' ', '+').Deserialize<DatabaseSource>();

			// save to output stream
			string mimeType, fileName;
			byte[] bytes = source.GetBytes(out mimeType, out fileName);
			context.Response.ContentType = mimeType;

			fileName = fileName.Replace(" ", "-");
			context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));

			context.Response.OutputStream.Write(bytes, 0, bytes.Length);
			context.Response.Flush();
		}

		#endregion
	}
}
