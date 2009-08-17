using System.Collections.Generic;
using Zeus.Collections;

namespace Zeus.ContentProperties
{
	public class PropertyCollectionDictionary : NamedCollectionDictionary<PropertyCollection>
	{
		public PropertyCollectionDictionary(IList<PropertyCollection> internalList)
			: base(internalList)
		{

		}
	}
}