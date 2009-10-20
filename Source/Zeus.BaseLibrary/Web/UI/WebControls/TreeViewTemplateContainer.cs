using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	public class TreeViewTemplateContainer : SimpleTemplateContainer
	{
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeNode DataItem
		{
			get;
			set;
		}
	}
}
