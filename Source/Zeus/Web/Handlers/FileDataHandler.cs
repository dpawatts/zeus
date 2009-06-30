using System;
using System.Web;
using Zeus.Admin;
using Zeus.ContentProperties;
using Zeus.FileSystem;

namespace Zeus.Web.Handlers
{
	public class FileDataHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			ContentItem contentItem = Context.Current.Resolve<Navigator>().Navigate(context.Request.QueryString["Path"]);
			if (contentItem == null)
				return;

			FileData fileData;
			if (!string.IsNullOrEmpty(context.Request.QueryString["DetailCollectionName"]))
			{
				PropertyCollection propertyCollection = contentItem.GetDetailCollection(context.Request.QueryString["DetailCollectionName"], false);
				if (propertyCollection == null)
					return;

				if (string.IsNullOrEmpty(context.Request.QueryString["Index"]))
					return;

				fileData = propertyCollection[Convert.ToInt32(context.Request.QueryString["Index"])] as FileData;
			}
			else
			{
				fileData = contentItem.GetDetail(context.Request.QueryString["DetailName"]) as FileData;
			}
			if (fileData == null)
				return;

			context.Response.ContentType = fileData.ContentType;
			context.Response.OutputStream.Write(fileData.Data, 0, fileData.Data.Length);
			context.Response.Flush();
		}
	}
}
