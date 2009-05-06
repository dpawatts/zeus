using System;

namespace Zeus.ContentProperties
{
	public class IntegerPropertyDataTypeAttribute : BasePropertyDataTypeAttribute
	{
		public override bool IsDefaultPropertyDataTypeForType(Type type)
		{
			return (type == typeof(int)) || type.IsEnum;
		}
	}
}