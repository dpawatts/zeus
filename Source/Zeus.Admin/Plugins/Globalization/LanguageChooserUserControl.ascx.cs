using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.Globalization
{
	[AjaxMethodProxyID(Alias = "LanguageChooser", IDMode = AjaxMethodProxyIDMode.Alias)]
	public partial class LanguageChooserUserControl : PluginUserControlBase
	{
		[AjaxMethod]
		public void ChangeLanguage(string languageCode)
		{
			Zeus.Context.AdminManager.CurrentAdminLanguageBranch = languageCode;
			Refresh(Find.StartPage);
		}
	}
}