using System.Web.UI;
using Ext.Net;
using Zeus.BaseLibrary.Web.UI;
using Button = System.Web.UI.WebControls.Button;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class ToolbarButton : Button
	{
		public Icon Icon
		{
			get { return (Icon) (ViewState["Icon"] ?? Icon.BulletBlue); }
			set { ViewState["Icon"] = value; }
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			AddAttributesToRender(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.Button);
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			/*
			 * <img src="/images/icons/tick.png" alt="" />
				 Save
			 */
			writer.AddAttribute(HtmlTextWriterAttribute.Src, Utility.GetCooliteIconUrl(Icon));
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			writer.Write(Text);
		}
	}
}