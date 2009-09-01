using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType("Search Results Page")]
	[RestrictParents(typeof(Shop))]
	public class SearchResultsPage : BasePage
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(SearchResultsPage), "Zeus.AddIns.ECommerce.Icons.magnifier.png"); }
		}
	}
}