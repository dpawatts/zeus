using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeus.Linq.Filters
{
	public class NavigationFilter : CompositeFilter
	{
		public NavigationFilter()
			: base(new VisibleFilter(), new PublishedFilter())
		{

		}
	}
}
