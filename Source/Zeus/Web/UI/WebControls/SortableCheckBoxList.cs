using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	public sealed class SortableCheckBoxList : CheckBoxList
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterClientScriptResource(typeof(SortableCheckBoxList), "Zeus.Web.Resources.ui.core.js");
			Page.ClientScript.RegisterClientScriptResource(typeof(SortableCheckBoxList), "Zeus.Web.Resources.ui.sortable.js");

			string script = "$(function() { $('#" + ClientID + "').sortable({ axis: 'y', containment: '#" + ClientID + "', handle: '.dragHandle' }); });";
			Page.ClientScript.RegisterStartupScript(typeof(SortableCheckBoxList), ClientID, script, true);
		}

		protected override void RenderItem(ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer)
		{
			writer.Write("<div><span class=\"ui-icon ui-icon-arrowthick-2-n-s dragHandle\" style=\"float:left\"></span>");
			base.RenderItem(itemType, repeatIndex, repeatInfo, writer);
			writer.Write("</div>");
		}
	}
}
