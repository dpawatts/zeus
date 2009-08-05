using System;
using System.Web.UI;
using System.Collections;
using System.ComponentModel.Design;
using System.Web.Compilation;

namespace Isis.Web.UI.WebControls
{
	public class ConditionalMultiViewControlBuilder : ControlBuilder
	{
		public Type DataItemType
		{
			get;
			private set;
		}

		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			string dataItemTypeName = attribs["DataItemTypeName"] as string;
			if (!string.IsNullOrEmpty(dataItemTypeName))
			{
				if (this.InDesigner)
				{
					ITypeResolutionService typeResolutionService = (ITypeResolutionService) this.ServiceProvider.GetService(typeof(ITypeResolutionService));
					this.DataItemType = typeResolutionService.GetType(dataItemTypeName);
				}
				else
				{
					this.DataItemType = BuildManager.GetType(dataItemTypeName, true);
				}
			}

			base.Init(parser, parentBuilder, type, tagName, id, attribs);
		}
	}
}
