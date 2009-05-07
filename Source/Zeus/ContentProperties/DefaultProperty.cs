using System;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class DefaultProperty : IContentProperty
	{
		public DefaultProperty()
		{
			Shared = true;
			SortOrder = int.MaxValue;
			Title = "[None]";
		}

		public string Name { get; set; }
		public bool Shared { get; set; }
		public int SortOrder { get; set; }
		public string Title { get; set; }

		public Type PropertyType { get; set; }

		public IDisplayer GetDefaultDisplayer()
		{
			throw new NotImplementedException();
		}

		public IEditor GetDefaultEditor()
		{
			throw new NotImplementedException();
		}

		public PropertyData CreatePropertyData(ContentItem enclosingItem, object value)
		{
			PropertyData propertyData = (PropertyData) Activator.CreateInstance(GetPropertyDataType());
			propertyData.Name = Name;
			propertyData.EnclosingItem = enclosingItem;
			propertyData.Value = value;
			return propertyData;
		}

		public Type GetPropertyDataType()
		{
			// For underlying property type "string", return typeof(StringProperty), etc.
			Type propertyDataType = Context.Current.Resolve<IContentPropertyManager>().GetDefaultPropertyDataType(PropertyType);
			if (propertyDataType != null)
				return propertyDataType;

			throw new ZeusException("No default PropertyData type is registered for property type '" + PropertyType + "'");
		}
	}
}