using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.Web.UI;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class ToolbarHyperLink : HyperLink
	{
		public string ImageResourceName
		{
			get { return ViewState["ImageResourceName"] as string ?? string.Empty; }
			set { ViewState["ImageResourceName"] = value; }
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			/*
			 * <img src="/images/icons/tick.png" alt="" />
				 Save
			 */
			writer.AddAttribute(HtmlTextWriterAttribute.Src, WebResourceUtility.GetUrl(Page.GetType().BaseType, ImageResourceName));
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			writer.Write(Text);
		}
	}
}