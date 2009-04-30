using System;

namespace Zeus.ContentProperties
{
	public class ContentPropertyManager : IContentPropertyManager
	{
		public IContentProperty CreateProperty(string name, Type valueType)
		{
			return new SimpleProperty { Name = name, Shared = true, PropertyType = valueType };
		}
	}
}