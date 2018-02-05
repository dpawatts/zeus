using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.BaseLibrary;
using Zeus.Templates.ContentTypes.ReferenceData;

namespace Zeus.Templates.Mvc.Html
{
	public static class ReferenceDataExtensions
	{
		public static IEnumerable<SelectListItem> CountryList(this HtmlHelper html, object selectedValue)
		{
			CountryList countryList = (CountryList)Find.RootItem.GetChild("system").GetChild("reference-data").GetChild("countries");
			return countryList.GetChildren<Country>().Select(c => new SelectListItem
			{
				Value = c.ID.ToString(),
				Text = c.Title,
				Selected = c.ID.Equals(selectedValue)
			});
		}

		public static IEnumerable<SelectListItem> EnumList(this HtmlHelper html, Type enumType, object selectedValue)
		{
			string selectedValueString = (selectedValue != null) ? selectedValue.ToString() : null;
			enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
			return Enum.GetNames(enumType).Select(name =>
				new SelectListItem
				{
					Value = name,
					Text = EnumHelper.GetEnumValueDescription(enumType, name),
					Selected = (name == selectedValueString)
				});
		}
	}
}