using System;
using System.IO;
using System.Threading;
using System.Web;

namespace Zeus.Web.Handlers
{
	public abstract class BaseFileUploadHandler : IHttpHandler
	{
		private const string UploadRootFolder = "_Zeus.FileUpload";

		public abstract void ProcessRequest(HttpContext context);

		protected static string GetUploadFolder(Guid identifier, bool firstChunk)
		{
			string uploadFolderPath = GetUploadFolder(identifier.ToString());
			if (firstChunk)
				Directory.CreateDirectory(uploadFolderPath);

			return uploadFolderPath;
		}

		public static string GetUploadFolder(string identifier)
		{
			string tempFolderPath = HttpRuntime.AppDomainAppPath;

			string uploadRootFolderPath = Path.Combine(tempFolderPath, UploadRootFolder);
			if (!Directory.Exists(uploadRootFolderPath))
				Directory.CreateDirectory(uploadRootFolderPath);

			string uploadFolderPath = Path.Combine(uploadRootFolderPath, identifier);

			return uploadFolderPath;
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}