using System.Collections.Generic;
using Zeus.Templates.ContentTypes.News;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class LatestNewsSummaryViewModel : ViewModel<LatestNewsSummary>
	{
		public LatestNewsSummaryViewModel(LatestNewsSummary currentItem, IEnumerable<NewsItem> newsItems)
			: base(currentItem)
		{
			NewsItems = newsItems;
		}

		public IEnumerable<NewsItem> NewsItems { get; set; }
	}
}