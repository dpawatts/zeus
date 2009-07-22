using System.Collections;
using System.Web;

namespace Zeus.Web.Handlers
{
	public class CacheHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write("<table>");
			foreach (DictionaryEntry dictionaryEntry in context.Cache)
			{
				context.Response.Write("<tr>");
				context.Response.Write("<td>" + dictionaryEntry.Key +"</td>");
				context.Response.Write("<td>" + dictionaryEntry.Value + "</td>");
				context.Response.Write("</tr>");
			}
			context.Response.Write("</table>");
		}
	}
}
