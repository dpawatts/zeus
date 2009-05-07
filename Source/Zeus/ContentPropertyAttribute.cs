using System;
using System.Collections.Generic;
using System.Reflection;
using Isis;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus
{
	public class ContentPropertyAttribute : BaseContentPropertyAttribute, IPropertyAwareAttribute
	{
		#region Fields

		private Type _propertyDataType;

		#endregion

		#region Constructors

		public ContentPropertyAttribute(Type propertyDataType, string title, int sortOrder)
			: this(title, sortOrder)
		{
			_propertyDataType = propertyDataType;
		}

		public ContentPropertyAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		#endregion

		#region Properties

		public PropertyInfo UnderlyingProperty { get; set; }

		#endregion

		#region Methods

		protected override IEditor GetDefaultEditorInternal(Type propertyType)
		{
			PropertyData propertyData = (PropertyData) Activator.CreateInstance(GetPropertyDataType());
			return propertyData.GetDefaultEditor(Title, SortOrder, propertyType);
		}

		public override Type GetPropertyDataType()
		{
			// If type has been set explicitly, use that.
			if (_propertyDataType != null)
				return _propertyDataType;

			// For underlying property type "string", return typeof(StringProperty), etc.
			Type propertyDataType = Context.Current.Resolve<IContentPropertyManager>().GetDefaultPropertyDataType(UnderlyingProperty.PropertyType);
			if (propertyDataType != null)
				return propertyDataType;

			throw new ZeusException("No default PropertyData type is registered for property type '" + UnderlyingProperty.PropertyType + "'");
		}

		protected override Type GetPropertyType()
		{
			return UnderlyingProperty.PropertyType;
		}

		#endregion
	}
}