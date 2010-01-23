using System.Web.UI.WebControls;
using Coolite.Ext.UX;
using Ext.Net;
using Zeus.Configuration;
using Zeus.Globalization;
using Zeus.Globalization.ContentTypes;

namespace Zeus.Admin.Plugins.Globalization
{
	public class LanguageChooserTreePlugin : TreePluginBase
	{
		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.Globalization.LanguageChooserUserControl.ascx") }; }
		}

		public override void ModifyTree(TreePanel treePanel, IMainInterface mainInterface)
		{
			if (!Context.Current.Resolve<GlobalizationSection>().Enabled)
				return;

			// Setup tree bottom toolbar.
			Toolbar bottomToolbar = new Toolbar();
			treePanel.BottomBar.Add(bottomToolbar);

			ToolbarTextItem textItem = new ToolbarTextItem { Text = "Language: " };
			bottomToolbar.Items.Add(textItem);

			IconCombo comboBox = new IconCombo
			{
				EmptyText = "Select...",
				Width = Unit.Pixel(100),
				Editable = false
			};
			comboBox.Listeners.Select.Handler = "Ext.net.DirectMethods.LanguageChooser.ChangeLanguage(record.get('value'), { url: '/admin/default.aspx', success: function(result) { stbStatusBar.setStatus({ text: 'Changed language', iconCls: '', clear: true }); } });";

			foreach (Language language in Context.Current.Resolve<ILanguageManager>().GetAvailableLanguages())
			{
				IconComboListItem listItem = new IconComboListItem(language.Title, language.Name, language.IconUrl);
				comboBox.Items.Add(listItem);
				if (language.Name == Context.AdminManager.CurrentAdminLanguageBranch)
				{
					comboBox.SelectedItem.Text = listItem.Text;
					comboBox.SelectedItem.Value = listItem.Value;
				}
			}

			bottomToolbar.Items.Add(comboBox);
		}
	}
}