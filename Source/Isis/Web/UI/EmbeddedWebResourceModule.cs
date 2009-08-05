using System;
using System.IO;
using System.Web;
using Isis.ExtensionMethods.IO;

namespace Isis.Web.UI
{
	public class EmbeddedWebResourceModule : IHttpModule
	{
		public void Init(HttpApplication application)
		{
			application.BeginRequest += application_BeginRequest;
		}

		private void application_BeginRequest(object sender, EventArgs e)
		{
			// Check if we have an embedded resource matching the requested path.
			string absolutePath = HttpContext.Current.Request.Path;
			if (EmbeddedWebResourceUtility.Files.Contains(absolutePath))
			{
				EmbeddedWebResourceFile embeddedWebResourceFile = EmbeddedWebResourceUtility.Files[absolutePath];
				using (Stream stream = embeddedWebResourceFile.Open())
				{
					byte[] bytes = stream.ReadAllBytes();
					HttpContext.Current.Response.ContentType = embeddedWebResourceFile.ContentType;
					HttpContext.Current.Response.BinaryWrite(bytes);
					HttpContext.Current.Response.End();
				}
			}
		}

		public void Dispose()
		{
			
		}
	}
}