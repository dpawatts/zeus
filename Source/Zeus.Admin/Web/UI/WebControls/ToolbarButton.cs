using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web.UI;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class ToolbarButton : Button
	{
		public string ImageResourceName
		{
			get { return ViewState["ImageResourceName"] as string ?? string.Empty; }
			set { ViewState["ImageResourceName"] = value; }
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
			writer.AddAttribute(HtmlTextWriterAttribute.Src, WebResourceUtility.GetUrl(Page.GetType().BaseType, ImageResourceName));
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			writer.Write(Text);
		}
	}
}