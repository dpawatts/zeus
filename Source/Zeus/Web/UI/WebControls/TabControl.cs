using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class TabControl : Panel
	{
		private Dictionary<TabItem, HtmlAnchor> _tabItemAnchorDictionary;
		private HtmlGenericControl _ul;

		protected override void AddedControl(System.Web.UI.Control control, int index)
		{
			base.AddedControl(control, index);

			if (control is TabItem)
			{
				if (_ul == null)
				{
					_ul = new HtmlGenericControl("ul") { EnableViewState = false };
					Controls.AddAt(0, _ul);

					_tabItemAnchorDictionary = new Dictionary<TabItem, HtmlAnchor>();
				}

				TabItem tabItem = (TabItem) control;

				HtmlGenericControl li = new HtmlGenericControl("li");
				_ul.Controls.Add(li);

				HtmlAnchor a = new HtmlAnchor();
				li.Controls.Add(a);

				a.HRef = "#" + tabItem.ClientID;
				a.InnerText = tabItem.ToolTip;

				_tabItemAnchorDictionary.Add(tabItem, a);
			}
		}

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
				
				jQuery(document).ready(function() {{
					jQuery('#{0}').tabs(
						{{
							show: function(event, ui) {{ handlePostbacks(ui.panel.id); }}
						}}
					);
				}});", ClientID);
			Page.ClientScript.RegisterStartupScript(typeof(TabControl), ClientID, script, true);

			foreach (TabItem tabItem in Controls.OfType<TabItem>())
				_tabItemAnchorDictionary[tabItem].HRef = "#" + tabItem.ClientID;

			Visible = Controls.OfType<TabItem>().Any(ti => ti.Controls.Count > 0);
		}
	}
}
