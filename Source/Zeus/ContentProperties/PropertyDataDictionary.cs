using System.Collections.Generic;
using Zeus.Collections;

namespace Zeus.ContentProperties
{
	public class PropertyDataDictionary : NamedCollectionDictionary<PropertyData>
	{
		public PropertyDataDictionary(IList<PropertyData> internalList)
			: base(internalList)
		{

		}
	}
}