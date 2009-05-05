using System;

namespace Zeus.ContentProperties
{
	public class PropertyDataTypeAttribute : BasePropertyDataTypeAttribute
	{
		private readonly Type _valueType;

		public PropertyDataTypeAttribute(Type valueType)
		{
			_valueType = valueType;
		}

		public PropertyDataTypeAttribute()
		{
		}

		public override bool IsDefaultPropertyDataTypeForType(Type type)
		{
			return _valueType == type;
		}
	}
}