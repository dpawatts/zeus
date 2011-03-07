using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Web.Compilation;
using System.Web.UI;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.Web.UI.WebControls
{
	public class TypedGridViewControlBuilder : ControlBuilder
	{
		internal Type DataItemType
		{
			get;
			private set;
		}

		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			string dataItemTypeName = attribs["DataItemTypeName"] as string;
			if (!string.IsNullOrEmpty(dataItemTypeName))
			{
				if (InDesigner)
				{
					ITypeResolutionService typeResolutionService = ServiceProvider.GetService<ITypeResolutionService>();
					DataItemType = typeResolutionService.GetType(dataItemTypeName);
				}
				else
				{
					DataItemType = BuildManager.GetType(dataItemTypeName, true);
				}
			}

			Type fakeType = new TypedGridViewFakeType(DataItemType);
			base.Init(parser, parentBuilder, fakeType, tagName, id, attribs);
		}
	}
}