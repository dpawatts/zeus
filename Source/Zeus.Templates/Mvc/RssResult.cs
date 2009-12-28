using System;
using System.Text;
using System.Web.Mvc;
using Argotic.Common;
using Argotic.Syndication;
using Zeus.Templates.Services.Syndication;

namespace Zeus.Templates.Mvc
{
	public class RssResult : SyndicationResultBase
	{
		protected override string ContentType
		{
			get { return "application/rss+xml"; }
		}

		public RssResult(Feed feed)
			: base(feed)
		{
			
		}

		protected override void WriteXml(ControllerContext context)
		{
			RssFeed feed = new RssFeed();
			feed.Channel.Title = Feed.Title;
			feed.Channel.Link = GetLink(Feed.Url);
			feed.Channel.Description = Feed.Tagline;
			feed.Channel.PublicationDate = Feed.Published;
			feed.Channel.LastBuildDate = DateTime.Now;
			feed.Channel.Generator = "Zeus CMS";
			feed.Channel.ManagingEditor = Feed.Author;

			foreach (ISyndicatable syndicatable in Feed.Items)
			{
				RssItem item = new RssItem();
				item.Title = syndicatable.Title;
				item.Link = GetLink(syndicatable.Url);
				item.Description = syndicatable.Summary;
				item.PublicationDate = syndicatable.Published;
				feed.Channel.AddItem(item);
			}

			SyndicationResourceSaveSettings settings = new SyndicationResourceSaveSettings
			{
				CharacterEncoding = new UTF8Encoding(false)
			};

			feed.Save(context.HttpContext.Response.OutputStream, settings);
		}

		private static Uri GetLink(string url)
		{
			return new Uri(Context.Current.WebContext.GetFullyQualifiedUrl(url));
		}
	}
}