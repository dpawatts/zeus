using System;
using System.Web.Mvc;

namespace Zeus.AddIns.Blogs.Mvc.Html
{
	public static class TrackbackExtensions
	{
		public static string TrackbackCode(this HtmlHelper html)
		{
			throw new NotImplementedException();

			return @"
<!--
<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
	xmlns:dc=""http://purl.org/dc/elements/1.1/""
	xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"">
  <rdf:Description rdf:about=""http://nayyeri.net/blog/results-of-first-waegis-survey/""
		dc:identifier=""http://nayyeri.net/blog/results-of-first-waegis-survey/""
		dc:title=""Results of First Waegis Survey""
		trackback:ping=""http://nayyeri.net/trackback.ashx?id=882"" />
</rdf:RDF>";
		}
	}
}