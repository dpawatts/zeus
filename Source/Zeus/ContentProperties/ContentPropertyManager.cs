using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Engine;

namespace Zeus.ContentProperties
{
	public class ContentPropertyManager : IContentPropertyManager
	{
		private IEnumerable<BasePropertyDataTypeAttribute> _propertyDataTypes;

		public ContentPropertyManager(IPluginFinder<BasePropertyDataTypeAttribute> pluginFinder)
		{
			_propertyDataTypes = pluginFinder.GetPlugins().OrderBy(p => p.SortOrder);
		}

		public IContentProperty CreateProperty(string name, Type valueType)
		{
			return new DefaultProperty { Name = name, Shared = true, PropertyType = valueType };
		}

		public Type GetDefaultPropertyDataType(Type type)
		{
			Type underlyingType = type.GetTypeOrUnderlyingType();
			foreach (BasePropertyDataTypeAttribute propertyDataTypeAttribute in _propertyDataTypes)
				if (propertyDataTypeAttribute.IsDefaultPropertyDataTypeForType(underlyingType))
					return propertyDataTypeAttribute.ContextType;
			return null;
		}

		public PropertyData CreatePropertyDataObject(Type type)
		{
			Type propertyDataType = GetDefaultPropertyDataType(type);
			return (PropertyData) Activator.CreateInstance(propertyDataType);
		}
	}
}