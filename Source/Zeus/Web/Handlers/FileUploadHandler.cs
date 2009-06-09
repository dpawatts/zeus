using System;
using System.IO;
using System.Threading;
using System.Web;

namespace Zeus.Web.Handlers
{
	public class FileUploadHandler : IHttpHandler
	{
		private const string UploadRootFolder = "_Zeus.FileUpload";
		private const string TempExtension = "_temp";

		public void ProcessRequest(HttpContext context)
		{
			if (context.Request.InputStream.Length == 0)
				throw new ArgumentException("No file input");

			// Get parameters from querystring.
			Guid identifier = new Guid(context.Request.QueryString["identifier"]);
			string fileName = context.Request.QueryString["file"];
			bool lastChunk = string.IsNullOrEmpty(context.Request.QueryString["last"]) ? true : bool.Parse(context.Request.QueryString["last"]);
			bool firstChunk = string.IsNullOrEmpty(context.Request.QueryString["first"]) ? true : bool.Parse(context.Request.QueryString["first"]);
			long startByte = string.IsNullOrEmpty(context.Request.QueryString["offset"]) ? 0 : long.Parse(context.Request.QueryString["offset"]); ;

			// Work out (and create if necessary) the path to upload to.
			string uploadFolder = GetUploadFolder(identifier, firstChunk);
			string tempUploadPath = Path.Combine(uploadFolder, fileName + TempExtension);

			// Save this chunk of the file.
			using (FileStream fs = File.Open(tempUploadPath, FileMode.Append))
			{
				SaveFile(context.Request.InputStream, fs);
				fs.Close();
			}

			// Is it the last chunk? Then finish up...
			if (lastChunk)
			{
				// Rename file to original file.
				string finalUploadPath = Path.Combine(uploadFolder, fileName);
				File.Move(tempUploadPath, finalUploadPath);
			}
		}

		private static string GetUploadFolder(Guid identifier, bool firstChunk)
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

		/// <summary>
		/// Save the contents of the Stream to a file
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="fs"></param>
		private static void SaveFile(Stream stream, Stream fs)
		{
			byte[] buffer = new byte[4096];
			int bytesRead;
			while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				fs.Write(buffer, 0, bytesRead);
			}
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}