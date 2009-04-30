using System.Collections.Generic;
using Zeus.Web;

namespace Zeus.Persistence.Specifications
{
	/// <summary>
	/// Filters based on the <see cref="ContentItem.ZoneName"/> property.
	/// </summary>
	public class ZoneSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public ZoneSpecification(string zoneName)
			: base(ci => ci.ZoneName == zoneName)
		{
			
		}
	}
}
