using System;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(SearchResultsPage), AreaName = ECommerceAreaRegistration.AREA_NAME)]
	public class SearchResultsPageController : ZeusController<SearchResultsPage>
	{
		[ActionName("NotUsed")]
		public override ActionResult Index()
		{
			throw new NotSupportedException();
		}

		public ActionResult Index(string keywords, int? page, bool? viewAll)
		{
			if (keywords == null)
				keywords = string.Empty;

			var searchResults = Find.EnumerateAccessibleChildren(CurrentItem.Parent)
				.OfType<Product>()
				.Where(product => product.Title.Contains(keywords, StringComparison.InvariantCultureIgnoreCase))
				.AsPageable(!(viewAll ?? false), page ?? 1, 8);
			return View(new SearchResultsPageViewModel(CurrentItem, keywords, searchResults));
		}
	}
}