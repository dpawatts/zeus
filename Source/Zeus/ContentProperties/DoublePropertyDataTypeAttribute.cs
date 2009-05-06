using System;

namespace Zeus.ContentProperties
{
	public class DoublePropertyDataTypeAttribute : BasePropertyDataTypeAttribute
	{
		public override bool IsDefaultPropertyDataTypeForType(Type type)
		{
			return (type == typeof(double)) || (type == typeof(float)) || (type == typeof(decimal));
		}
	}
}