using System;
using System.Web;
using System.IO;

namespace Zeus.BaseLibrary.Web.Handlers
{
	public class FlashVideoStreamingHandler  : IHttpHandler
	{
		// FLV header
		private static readonly byte[] _flvHeader = HexToByte("464C5601010000000900000009");

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				int pos;
				int length;

				// Check start parameter if present
				using (FileStream fs = new FileStream(context.Request.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					string qs = context.Request.Params["start"];

					if (string.IsNullOrEmpty(qs))
					{
						pos = 0;
						length = Convert.ToInt32(fs.Length);
					}
					else
					{
						pos = Convert.ToInt32(qs);
						length = Convert.ToInt32(fs.Length - pos) + _flvHeader.Length;
					}

					// add HTTP header stuff: cache, content type and length        
					context.Response.Cache.SetCacheability(HttpCacheability.Public);
					context.Response.Cache.SetLastModified(DateTime.Now);

					context.Response.AppendHeader("Content-Type", "video/x-flv");
					context.Response.AppendHeader("Content-Length", length.ToString());

					// append FLV header when sending partial file
					if (pos > 0)
					{
						context.Response.OutputStream.Write(_flvHeader, 0, _flvHeader.Length);
						fs.Position = pos;
					}

					// read buffer and write stream to the response stream
					const int BUFFER_SIZE = 16384;
					byte[] buffer = new byte[BUFFER_SIZE];

					int count = fs.Read(buffer, 0, BUFFER_SIZE);
					while (count > 0)
					{
						if (context.Response.IsClientConnected)
						{
							context.Response.OutputStream.Write(buffer, 0, count);
							count = fs.Read(buffer, 0, BUFFER_SIZE);
						}
						else
						{
							count = -1;
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}

		public bool IsReusable
		{
			get { return true; }
		}

		private static byte[] HexToByte(string hexString)
		{
			byte[] returnBytes = new byte[hexString.Length / 2];
			for (int i = 0; i < returnBytes.Length; i++)
				returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			return returnBytes;
		}
	}
}