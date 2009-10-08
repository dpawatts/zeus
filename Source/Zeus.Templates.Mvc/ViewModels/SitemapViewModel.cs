using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class SitemapViewModel : ViewModel<Sitemap>
	{
		public SitemapViewModel(Sitemap currentItem, ContentItem startPage)
			: base(currentItem)
		{
			StartPage = startPage;
		}

		public ContentItem StartPage { get; set; }
	}
}