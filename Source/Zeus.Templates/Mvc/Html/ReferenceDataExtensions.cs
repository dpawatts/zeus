using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes.ReferenceData;

namespace Zeus.Templates.Mvc.Html
{
	public static class ReferenceDataExtensions
	{
		public static IEnumerable<SelectListItem> CountryList(this HtmlHelper html, object selectedCountry)
		{
			CountryList countryList = (CountryList) Find.RootItem.GetChild("system").GetChild("reference-data").GetChild("countries");
			return countryList.GetChildren<Country>().Select(c => new SelectListItem
			{
				Value = c.ID.ToString(),
				Text = c.Title,
				Selected = c.ID.Equals(selectedCountry)
			});
		}
	}
}