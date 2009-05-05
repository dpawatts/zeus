using System;

namespace Zeus.ContentProperties
{
	public interface IContentPropertyManager
	{
		IContentProperty CreateProperty(string name, Type valueType);

		Type GetDefaultPropertyDataType(Type type);
		PropertyData CreatePropertyDataObject(Type type);
	}
}