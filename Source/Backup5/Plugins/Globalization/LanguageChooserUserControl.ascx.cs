using Ext.Net;

namespace Zeus.Admin.Plugins.Globalization
{
	[DirectMethodProxyID(Alias = "LanguageChooser", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class LanguageChooserUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void ChangeLanguage(string languageCode)
		{
			Zeus.Context.AdminManager.CurrentAdminLanguageBranch = languageCode;
			Refresh(Zeus.Context.Current.LanguageManager.GetTranslation(Find.StartPage, languageCode));
		}
	}
}