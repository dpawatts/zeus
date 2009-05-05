using System;

namespace Zeus.ContentProperties
{
	public class ObjectPropertyDataTypeAttribute : BasePropertyDataTypeAttribute
	{
		public ObjectPropertyDataTypeAttribute()
		{
			SortOrder = int.MaxValue;
		}

		public override bool IsDefaultPropertyDataTypeForType(Type type)
		{
			return true;
		}
	}
}