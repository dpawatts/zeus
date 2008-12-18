using System;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class CountFilter : ItemFilter
	{
		public int StartIndex
		{
			get;
			set;
		}

		public int MaxCount
		{
			get;
			set;
		}

		public CountFilter(int startIndex, int maxCount)
		{
			this.StartIndex = startIndex;
			this.MaxCount = maxCount;
		}

		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			return source.Skip(this.StartIndex).Take(this.MaxCount);
		}
	}
}
