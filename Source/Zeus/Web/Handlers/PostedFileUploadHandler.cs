using System;
using System.IO;
using System.Web;

namespace Zeus.Web.Handlers
{
	public class PostedFileUploadHandler : BaseFileUploadHandler
	{
		public override void ProcessRequest(HttpContext context)
		{
			HttpPostedFile postedFile = context.Request.Files["Filedata"];
			Guid identifier = new Guid(context.Request.QueryString["identifier"]);
			string fileName = context.Request.QueryString["file"];

			// Work out (and create if necessary) the path to upload to.
			string uploadFolder = GetUploadFolder(identifier, true);
			string finalUploadPath = Path.Combine(uploadFolder, fileName);
            postedFile.SaveAs(finalUploadPath);

			context.Response.Write("1");
		}
	}
}