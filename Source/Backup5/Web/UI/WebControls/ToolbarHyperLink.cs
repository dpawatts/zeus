using System.Web.UI;
using Ext.Net;
using Zeus.BaseLibrary.Web.UI;
using HyperLink = System.Web.UI.WebControls.HyperLink;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class ToolbarHyperLink : HyperLink
	{
		public Icon Icon
		{
			get { return (Icon) (ViewState["Icon"] ?? Icon.BulletBlue); }
			set { ViewState["Icon"] = value; }
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