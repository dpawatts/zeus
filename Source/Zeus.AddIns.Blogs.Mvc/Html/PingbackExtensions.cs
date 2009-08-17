using System;
using System.Web.Mvc;

namespace Zeus.AddIns.Blogs.Mvc.Html
{
	public static class PingbackExtensions
	{
		public static string PingbackLink(this HtmlHelper html)
		{
			throw new NotImplementedException();

			return "<link rel=\"pingback\" href=\"http://localhost:10220/Pingback.aspx\">";
		}
	}
}