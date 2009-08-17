using System;
using System.Web;
using System.Xml;

namespace Zeus.AddIns.Blogs.Web.Trackbacks
{
	public class TrackbackHandler : IHttpHandler
	{
		#region IHttpHandler Members

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			var request = context.Request;
			var response = context.Response;

			response.Buffer = false;
			response.Clear();
			response.ContentType = "application/xml";

			var writer = XmlWriter.Create(response.Output);

			if (!string.IsNullOrEmpty(request["id"]))
			{
				var blogTitle = string.Empty;
				if (!string.IsNullOrEmpty(request["blog_name"]))
					blogTitle = request["blog_name"];

				var url = string.Empty;
				if (!string.IsNullOrEmpty(request["url"]))
					url = request["url"];

				var title = string.Empty;
				if (!string.IsNullOrEmpty(request["title"]))
					title = request["title"];

				var excerpt = string.Empty;
				if (!string.IsNullOrEmpty(request["excerpt"]))
					excerpt = request["excerpt"];

				if (request.HttpMethod == "POST")
				{
					// Store trackback based on the id parameter
					throw new NotImplementedException();
					GenerateSuccessResponse(writer);
				}
				else
				{
					GenerateErrorResponse(2, "Only HTTP POST verb can be used to send trackbacks", writer);
				}
			}
			else
			{
				GenerateErrorResponse(1, "Item identifier is missing", writer);
			}
		}

		private static void GenerateSuccessResponse(XmlWriter writer)
		{
			writer.WriteStartElement("response");
			writer.WriteElementString("error", "0");
			writer.WriteStartElement("rss");
			writer.WriteAttributeString("version", "0.91");
			writer.WriteStartElement("channel");

			writer.WriteElementString("title", "Sample Page");
			writer.WriteElementString("link", "http://localhost:5935/default.aspx");
			writer.WriteElementString("description", "This is just a sample page.");
			writer.WriteElementString("language", "");

			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();

			writer.Flush();
			writer.Close();
		}

		private static void GenerateErrorResponse(int number, string message, XmlWriter writer)
		{
			writer.WriteStartElement("response");

			if (number > 0)
				writer.WriteElementString("error", number.ToString());
			if (!string.IsNullOrEmpty(message))
				writer.WriteElementString("message", message);

			writer.WriteEndElement();

			writer.Flush();
			writer.Close();
		}

		#endregion
	}
}