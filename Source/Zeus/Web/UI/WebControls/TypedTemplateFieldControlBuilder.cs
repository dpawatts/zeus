using System;
using System.Collections;
using System.Web.UI;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	public class TypedTemplateFieldControlBuilder : ControlBuilder
	{
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			// Nasty hack to get internal property value.
			TypedGridViewControlBuilder typedGridViewControlBuilder = parentBuilder.GetValue("ParentBuilder") as TypedGridViewControlBuilder;
			Type fakeType = type;
			if (typedGridViewControlBuilder != null)
			{
				Type dataItemType = typedGridViewControlBuilder.DataItemType;
				if (dataItemType != null)
					fakeType = new TypedTemplateFieldFakeType(dataItemType);
			}
			base.Init(parser, parentBuilder, fakeType, tagName, id, attribs);
		}
	}
}