using System.Web.UI;

namespace Isis.Web.UI.HtmlControls
{
	public class FieldSet : System.Web.UI.HtmlControls.HtmlContainerControl
	{
		public override string TagName
		{
			get { return "fieldset"; }
		}

		public string Legend
		{
			get { return (string) (ViewState["Legend"] ?? string.Empty); }
			set { ViewState["Legend"] = value; }
		}

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			base.RenderBeginTag(writer);
			if (this.Legend.Length > 0)
			{
				writer.Write("<legend>");
				writer.Write(this.Legend);
				writer.Write("</legend>");
			}
		}
	}
}
