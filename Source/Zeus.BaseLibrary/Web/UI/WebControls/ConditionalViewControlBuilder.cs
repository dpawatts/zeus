using System;
using System.Web.UI;
using System.Collections;
using System.ComponentModel.Design;
using System.Web.Compilation;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	public class ConditionalViewControlBuilder : ControlBuilder
	{
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			ConditionalMultiViewControlBuilder conditionalMultiViewControlBuilder = parentBuilder as ConditionalMultiViewControlBuilder;
			Type fakeType = type;
			if (conditionalMultiViewControlBuilder != null)
			{
				Type dataItemType = conditionalMultiViewControlBuilder.DataItemType;
				if (dataItemType != null)
					fakeType = new ConditionalViewFakeType(dataItemType);
			}
			base.Init(parser, parentBuilder, fakeType, tagName, id, attribs);
		}
	}
}
