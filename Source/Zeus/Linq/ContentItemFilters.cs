using System.Linq.Expressions;
using System;

namespace Zeus.Linq
{
	public static class ContentItemFilters
	{
		public static Expression<Func<TContentItem, bool>> DetailEquals<TContentItem>(string detailName, object value)
			where TContentItem : ContentItem
		{
			return ci => ci != null && ci.Details[detailName] != null && ci.Details[detailName].Value == value;
		}
	}
}
