using System.ComponentModel;
using Isis.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	public class MenuItem : SimpleTemplateContainer
	{
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ContentItem DataItem
		{
			get;
			set;
		}

		public string DataItemCssClass { get; set; }
	}
}