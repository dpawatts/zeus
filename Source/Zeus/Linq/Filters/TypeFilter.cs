using System;
using System.Linq;
using Isis.Linq;

namespace Zeus.Linq.Filters
{
	public class TypeFilter : ItemFilter
	{
		public Type[] Types
		{
			get;
			set;
		}

		public TypeFilter(params Type[] types)
		{
			this.Types = types;
		}

		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			IQueryable<ContentItem> result = source;
			foreach (Type type in this.Types)
				result = result.OfType(type);
			return result;
		}
	}
}
