using System.Text;
using System.Web.Mvc;
using Argotic.Common;
using Argotic.Syndication;

namespace Zeus.Templates.Mvc
{
	public class RssResult : ActionResult
	{
		public RssResult(RssFeed feed)
		{
			RssFeed = feed;
		}

		public RssFeed RssFeed { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "application/rss+xml";
			SyndicationResourceSaveSettings settings = new SyndicationResourceSaveSettings
			{
				CharacterEncoding = new UTF8Encoding(false)
			};
			RssFeed.Save(context.HttpContext.Response.OutputStream, settings);
		}
	}
}