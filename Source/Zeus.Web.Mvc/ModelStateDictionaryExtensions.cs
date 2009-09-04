using System.Globalization;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public static class ModelStateDictionaryExtensions
	{
		public static void Add(this ModelStateDictionary modelStateDictionary, string key, object value)
		{
			modelStateDictionary.Add(key, new ModelState
			{
				Value = new ValueProviderResult(value, (value != null) ? value.ToString() : string.Empty, CultureInfo.CurrentUICulture)
			});
		}
	}
}