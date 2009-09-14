using System;
using System.Web.Mvc;
using Argotic.Syndication;
using Zeus.Templates.Services.Syndication;
using Zeus.Web;
using RssFeed=Zeus.Templates.ContentTypes.RssFeed;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(RssFeed), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class RssFeedController : ZeusController<RssFeed>
	{
		private readonly IWebContext _webContext;

		public RssFeedController(IWebContext webContext)
		{
			_webContext = webContext;
		}

		public override ActionResult Index()
		{
			Argotic.Syndication.RssFeed feed = new Argotic.Syndication.RssFeed();
			feed.Channel.Title = CurrentItem.Title;
			feed.Channel.Link = GetLink(CurrentItem.Url);
			feed.Channel.Description = CurrentItem.Tagline;
			feed.Channel.PublicationDate = CurrentItem.Published ?? DateTime.Now;
			feed.Channel.LastBuildDate = DateTime.Now;
			feed.Channel.Generator = "Zeus CMS";
			feed.Channel.ManagingEditor = CurrentItem.Author;

			foreach (ISyndicatable syndicatable in CurrentItem.GetItems())
			{
				RssItem item = new RssItem();
				item.Title = syndicatable.Title;
				item.Link = GetLink(syndicatable.Url);
				item.Description = syndicatable.Summary;
				item.PublicationDate = syndicatable.Published ?? DateTime.Now;
				feed.Channel.AddItem(item);
			}

			return new RssResult(feed);
		}

		private Uri GetLink(string url)
		{
			return new Uri(_webContext.Url.HostUrl + url);
		}
	}
}