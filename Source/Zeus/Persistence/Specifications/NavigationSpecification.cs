using System.Collections.Generic;
using Zeus.Security;

namespace Zeus.Persistence.Specifications
{
	public class NavigationSpecification<T> : CompositeSpecification<T>
		where T : ContentItem
	{
		public NavigationSpecification()
			: base(new ISpecification<T>[] { new PageSpecification<T>(), new VisibleSpecification<T>(), new PublishedSpecification<T>(), new AccessSpecification<T>(Operations.Read) })
		{
			
		}
	}
}
