using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class TabControl : Panel
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterClientScriptResource(typeof(TabControl), "Zeus.Web.Resources.jQuery.ui.core.js");
			Page.ClientScript.RegisterClientScriptResource(typeof(TabControl), "Zeus.Web.Resources.jQuery.ui.tabs.js");

			Page.ClientScript.RegisterEmbeddedCssResource(typeof(TabControl), "Zeus.Web.Resources.jQuery.ui.css");

			string script = string.Format(@"
				function handlePostbacks(tabID) {{
					if (tabID && document.forms.length > 0) {{
						var f = document.forms[0];
						var index = f.action.indexOf('#');
						if (index > 0)
							f.action = f.action.substr(0,index) + '#' + tabID;
						else
							f.action += '#' + tabID;
					}}
				}}
				
				$(document).ready(function() {{
					$('#{0}').tabs(
						{{
							show: function(event, ui) {{ handlePostbacks(ui.panel.id); }}
						}}
					);
				}});", ClientID);
			Page.ClientScript.RegisterStartupScript(typeof(TabControl), ClientID, script, true);

			HtmlGenericControl ul = new HtmlGenericControl("ul");
			foreach (TabItem tabItem in Controls.OfType<TabItem>())
			{
				HtmlGenericControl li = new HtmlGenericControl("li");
				ul.Controls.Add(li);

				HtmlAnchor a = new HtmlAnchor();
				li.Controls.Add(a);

				a.HRef = "#" + tabItem.ClientID;
				a.InnerText = tabItem.ToolTip;
			}
			Controls.AddAt(0, ul);
		}
	}
}
