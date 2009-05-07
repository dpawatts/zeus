using System;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class XhtmlStringContentPropertyAttribute : BaseContentPropertyAttribute
	{
		public XhtmlStringContentPropertyAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override IEditor GetDefaultEditorInternal(Type propertyType)
		{
			return new HtmlTextBoxEditorAttribute(Title, SortOrder);
		}

		public override Type GetPropertyDataType()
		{
			return typeof(XhtmlStringProperty);
		}

		protected override Type GetPropertyType()
		{
			return typeof(string);
		}
	}
}