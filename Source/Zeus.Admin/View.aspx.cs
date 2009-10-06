using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Displayers;

namespace Zeus.Admin
{
	[ActionPluginGroup("ViewPreview", 30)]
	//[ActionPlugin("View", "View Details", Operations.Read, "ViewPreview", 1, null, "Zeus.Admin.View.aspx", "selected={selected}", Targets.Preview, "Zeus.Admin.Resources.application_view_detail.png")]
	public partial class View : PreviewFrameAdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			Title = "View \"" + SelectedItem.Title + "\"";

			// Get selected property from content item.
			ContentType contentType = Zeus.Context.Current.ContentTypes[SelectedItem.GetType()];
			foreach (IContentProperty property in contentType.Properties)
			{
				PlaceHolder plcDisplay = new PlaceHolder();
				Panel panel = new Panel { CssClass = "editDetail" };
				HtmlGenericControl label = new HtmlGenericControl("label");
				label.Attributes["class"] = "editorLabel";
				label.InnerText = property.Name;
				panel.Controls.Add(label);
				plcDisplay.Controls.Add(panel);
				plcDisplayers.Controls.Add(plcDisplay);

				IDisplayer displayer = contentType.GetDisplayer(property.Name);
				if (displayer != null)
				{
					//displayer.AddTo(this, contentItem, this.PropertyName);
					displayer.InstantiateIn(panel);
					displayer.SetValue(panel, SelectedItem, property.Name);
				}
				else
				{
					panel.Controls.Add(new LiteralControl("{No displayer}"));
				}
				panel.Controls.Add(new LiteralControl("&nbsp;"));
			}

			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(View), "Zeus.Admin.Assets.Css.edit.css");
			base.OnPreRender(e);
		}
	}
}
