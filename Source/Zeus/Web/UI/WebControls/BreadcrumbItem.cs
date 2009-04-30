using System.ComponentModel;
using Isis.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	public class BreadcrumbItem : SimpleTemplateContainer
	{
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ILink DataItem
		{
			get;
			set;
		}
	}
}