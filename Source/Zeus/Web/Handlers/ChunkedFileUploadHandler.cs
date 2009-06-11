using System;
using System.IO;
using System.Web;

namespace Zeus.Web.Handlers
{
	public class ChunkedFileUploadHandler : BaseFileUploadHandler
	{
		private const string TEMP_EXTENSION = "_temp";

		public override void ProcessRequest(HttpContext context)
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
			string tempUploadPath = Path.Combine(uploadFolder, fileName + TEMP_EXTENSION);

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
	}
}