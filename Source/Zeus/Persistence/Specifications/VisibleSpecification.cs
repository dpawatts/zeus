using System;

namespace Zeus.Persistence.Specifications
{
	public class VisibleSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public VisibleSpecification()
			: base(ci => ci.Visible)
		{

		}
	}
}
