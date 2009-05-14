using System;
using System.Linq;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Zeus.Admin.Web.UI.WebControls;
using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Web.UI;

[assembly: EmbeddedResourceFile("Zeus.Admin.Navigation.Tree.aspx", "Zeus.Admin")]
namespace Zeus.Admin.Navigation
{
	public partial class Tree : System.Web.UI.Page
	{
		protected override void OnPreRender(EventArgs e)
		{
			if (!IsPostBack)
			{
				ddlLanguages.SelectedValue = Zeus.Context.AdminManager.CurrentAdminLanguageBranch;
				plcLanguages.Visible = Zeus.Context.Current.Resolve<GlobalizationSection>().Enabled;
			}

			Page.ClientScript.RegisterJQuery();
			Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.Plugins.jquery.simpleTree.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.Plugins.jquery.easing.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.Plugins.jquery.easing.compatilibity.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.Plugins.jquery.dimensions.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.Plugins.jquery.contextMenu.js");
			if (Zeus.Context.AdminManager.TreeTooltipsEnabled)
				Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.Plugins.jquery.qtip.js", ResourceInsertPosition.BodyBottom);
			Page.ClientScript.RegisterJavascriptResource(typeof(Tree), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);

			Page.ClientScript.RegisterCssResource(typeof(Tree), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Tree), "Zeus.Admin.Assets.Css.tree.css");

			// As an optimisation, register all action plugins here.
			RegisterActionPlugins();

			plcTooltipsJavascript.Visible = Zeus.Context.AdminManager.TreeTooltipsEnabled;

			base.OnPreRender(e);
		}

		private void RegisterActionPlugins()
		{
			Page.ClientScript.RegisterStartupScript(typeof(Tree), "ActionPluginRegistration", "var actionPlugins = new Object();" + Environment.NewLine, true);
			foreach (ActionPluginGroupAttribute actionPluginGroup in Zeus.Context.Current.Resolve<IPluginFinder<ActionPluginGroupAttribute>>().GetPlugins().OrderBy(g => g.SortOrder))
				foreach (ActionPluginAttribute actionPlugin in Zeus.Context.Current.Resolve<IPluginFinder<ActionPluginAttribute>>().GetPlugins().Where(p => p.GroupName == actionPluginGroup.Name).OrderBy(p => p.SortOrder))
					RegisterActionPlugin(actionPlugin);
		}

		private void RegisterActionPlugin(ActionPluginAttribute actionPlugin)
		{
			string script = string.Format("actionPlugins['{5}'] = {{ Text : '{0}', PageUrl : '{1}', Target : '{2}', ImageUrl : '{3}', EnableCondition : '{4}' }};" + Environment.NewLine,
				actionPlugin.Text,
				actionPlugin.PageUrl.Replace("'", "\\'"),
				actionPlugin.Target,
				actionPlugin.ImageUrl,
				actionPlugin.JavascriptEnableCondition,
				actionPlugin.Name);
			Page.ClientScript.RegisterStartupScript(typeof(Tree), "ActionPlugin" + actionPlugin.Name, script, true);
		}

		protected void ddlLanguages_LanguageChanged(object sender, LanguageChangedEventArgs e)
		{
			Zeus.Context.AdminManager.CurrentAdminLanguageBranch = e.LanguageCode;
			Response.Redirect(Request.RawUrl);
		}
	}
}
