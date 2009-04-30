using System;
using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	/// <summary>
	/// Base class providing sorting capabilities to refiner attributes.
	/// </summary>
	public abstract class AbstractContentTypeRefiner : Attribute, ISortableRefiner
	{
		protected AbstractContentTypeRefiner()
		{
			RefinementOrder = RefineOrder.Middle;
		}

		#region ISortableRefiner Members

		public int RefinementOrder { get; set; }

		public abstract void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions);

		#endregion

		#region IComparable<ISortableRefiner> Members

		int IComparable<ISortableRefiner>.CompareTo(ISortableRefiner other)
		{
			return RefinementOrder - other.RefinementOrder;
		}

		#endregion
	}
}