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
            //see if the setting is in machine.config - the reason for having it there is that the same folder can be used for all sites,
            //reducing issues when moving sites around.  If no setting is found one will be created in the app folder, the issue with that
            //being that when a file is deleted the app domain restarts

            string uploadRootFolderPath = "";
            if (System.Configuration.ConfigurationSettings.AppSettings["ZeusUploadFolder"] != null)
            {
                uploadRootFolderPath = System.Configuration.ConfigurationSettings.AppSettings["ZeusUploadFolder"];
            }
            else
            {
                string tempFolderPath = HttpRuntime.AppDomainAppPath;
                uploadRootFolderPath = Path.Combine(tempFolderPath, UploadRootFolder);
            }

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