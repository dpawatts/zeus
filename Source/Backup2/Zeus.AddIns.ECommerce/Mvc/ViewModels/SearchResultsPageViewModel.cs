using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class SearchResultsPageViewModel : ViewModel<SearchResultsPage>
	{
		public SearchResultsPageViewModel(SearchResultsPage currentItem, string searchTerms,
			IPageable<Product> searchResults)
			: base(currentItem)
		{
			SearchTerms = searchTerms;
			SearchResults = searchResults;
		}

		public string SearchTerms { get; set; }
		public IPageable<Product> SearchResults { get; set; }
	}
}