using System;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class BaseContentPropertyAttribute : Attribute, IContentProperty
	{
		protected BaseContentPropertyAttribute(string title, int sortOrder)
			: this()
		{
			Title = title;
			SortOrder = sortOrder;
		}

		private BaseContentPropertyAttribute()
		{
			Shared = true;
		}

		public string Description { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this property is shared among all translations of a page.
		/// True if the property is shared, or false if the property is unique for each translation.
		/// </summary>
		public bool Shared { get; set; }

		public int SortOrder { get; set; }
		public string Title { get; set; }

		public virtual IDisplayer GetDefaultDisplayer()
		{
			return new LiteralDisplayerAttribute { Title = Title, Name = Name };
		}

		public virtual IEditor GetDefaultEditor()
		{
			//PropertyData propertyData = (PropertyData) Activator.CreateInstance(GetPropertyDataType());
			Type propertyType = GetPropertyType();
			IEditor editor = GetDefaultEditorInternal(propertyType);
			editor.Name = Name;
			editor.PropertyType = propertyType;
			editor.Shared = Shared;

			// TODO - clean this up.
			if (editor is AbstractEditorAttribute)
				((AbstractEditorAttribute) editor).Description = Description;

			return editor;
		}

		protected abstract IEditor GetDefaultEditorInternal(Type propertyType);

		public abstract Type GetPropertyDataType();
		protected abstract Type GetPropertyType();

		public PropertyData CreatePropertyData(ContentItem enclosingItem, object value)
		{
			Type propertyDataType = GetPropertyDataType();
			object untypedPropertyData;
			try
			{
				untypedPropertyData = Activator.CreateInstance(propertyDataType);
			}
			catch (Exception ex)
			{
				throw new Exception("Could not create instance of type '" + propertyDataType.FullName + "' for content property with name '" + Name + "'", ex);
			}
			PropertyData propertyData = (PropertyData) untypedPropertyData;
			propertyData.Name = Name;
			propertyData.EnclosingItem = enclosingItem;
			propertyData.Value = value;
			return propertyData;
		}

		public override bool Equals(object obj)
		{
			BaseContentPropertyAttribute other = obj as BaseContentPropertyAttribute;
			if (other == null)
				return false;

			return Title == other.Title
				&& SortOrder == other.SortOrder
				&& Description == other.Description
				&& Name == other.Name
				&& Shared == other.Shared;
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}