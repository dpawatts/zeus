using System;

namespace Zeus.ContentProperties
{
	public interface IContentPropertyManager
	{
		IContentProperty CreateProperty(string name, Type valueType);
	}
}