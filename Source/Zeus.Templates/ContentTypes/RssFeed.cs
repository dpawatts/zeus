using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Linq;
using Zeus.Templates.Services.Syndication;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Feed", Description = "An RSS feed that outputs XML with the latest feed data.")]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	public class RssFeed : PageContentItem, IFeed, INode
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Feed); }
		}

		[ContentProperty("Feed root", 90)]
		public virtual ContentItem FeedRoot
		{
			get { return (ContentItem)GetDetail("FeedRoot"); }
			set { SetDetail("FeedRoot", value); }
		}

		[ContentProperty("Number of items", 100)]
		public virtual int NumberOfItems
		{
			get { return (int)(GetDetail("NumberOfItems") ?? 10); }
			set { SetDetail("NumberOfItems", value, 10); }
		}

		[ContentProperty("Tagline", 110)]
		[TextBoxEditor(Required = true)]
		public virtual string Tagline
		{
			get { return (string)(GetDetail("Tagline") ?? string.Empty); }
			set { SetDetail("Tagline", value, string.Empty); }
		}

		[ContentProperty("Author", 120)]
		public virtual string Author
		{
			get { return (string)(GetDetail("Author") ?? string.Empty); }
			set { SetDetail("Author", value, string.Empty); }
		}

		[ContentProperty("RFC 3229 Delta Encoding Enabled", 130)]
		public virtual bool Rfc3229DeltaEncodingEnabled
		{
			get { return GetDetail("Rfc3229DeltaEncodingEnabled", true); }
			set { SetDetail("Rfc3229DeltaEncodingEnabled", value); }
		}

		public override string Url
		{
			get { return base.Url + "?hungry=yes"; }
		}

		public virtual IEnumerable<ISyndicatable> GetItems()
		{
			return Find.EnumerateAccessibleChildren(FeedRoot ?? Find.StartPage)
				.OfType<ISyndicatable>()
				.OfType<ContentItem>()
				.Navigable()
				.Where(ci => ci.GetDetail(SyndicatableDefinitionAppender.SyndicatableDetailName, true))
				.OrderByDescending(ci => ci.Published)
				.Take(NumberOfItems)
				.Cast<ISyndicatable>();
		}
	}
}