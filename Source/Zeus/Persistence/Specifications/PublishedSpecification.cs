using System;

namespace Zeus.Persistence.Specifications
{
	public class PublishedSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public PublishedSpecification()
			: base(ci => (ci.Published.HasValue && ci.Published.Value <= DateTime.Now)
				&& !(ci.Expires.HasValue && ci.Expires.Value < DateTime.Now))
		{

		}
	}
}
