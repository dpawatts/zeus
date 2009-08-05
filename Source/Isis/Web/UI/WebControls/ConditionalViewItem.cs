using System;
using System.Web.UI;
using System.ComponentModel;

namespace Isis.Web.UI.WebControls
{
	public class ConditionalViewItem : SimpleTemplateContainer
	{
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object DataItem
		{
			get;
			set;
		}
	}
}
