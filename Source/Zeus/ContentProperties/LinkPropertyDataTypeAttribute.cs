using System;

namespace Zeus.ContentProperties
{
	public class LinkPropertyDataTypeAttribute : BasePropertyDataTypeAttribute
	{
		public override bool IsDefaultPropertyDataTypeForType(Type type)
		{
			return typeof(ContentItem).IsAssignableFrom(type);
		}
	}
}