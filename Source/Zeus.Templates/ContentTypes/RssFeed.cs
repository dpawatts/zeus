using System.Collections.Generic;
using System.Linq;
using Zeus.Integrity;
using Zeus.Templates.Services.Syndication;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Feed", Description = "An RSS feed that outputs an xml with the latest feeds.")]
	[RestrictParents(typeof(BasePage))]
	public class RssFeed : BasePage, IFeed, INode
	{
		protected override string IconName
		{
			get { return "feed"; }
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

		public override string Url
		{
			get { return base.Url + "?hungry=yes"; }
		}

		public string PreviewUrl
		{
			get { return base.FindPath(PathData.DefaultAction).RewrittenUrl; }
		}

		public virtual IEnumerable<ISyndicatable> GetItems()
		{
			return Find.EnumerateAccessibleChildren(FeedRoot ?? Find.StartPage)
				.OfType<ISyndicatable>()
				.OfType<ContentItem>()
				.Where(ci => ci.GetDetail(SyndicatableDefinitionAppender.SyndicatableDetailName, true))
				.OrderByDescending(ci => ci.Published)
				.Take(NumberOfItems)
				.Cast<ISyndicatable>();
		}
	}
}