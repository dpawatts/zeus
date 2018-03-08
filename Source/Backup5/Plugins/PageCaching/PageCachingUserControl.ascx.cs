using System;
using Ext.Net;
using Zeus.Web.Caching;

namespace Zeus.Admin.Plugins.PageCaching
{
	[DirectMethodProxyID(Alias = "PageCaching", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class PageCachingUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void ShowDialog(int id)
		{
			ContentItem contentItem = Engine.Persister.Get(id);

			var window = new Window
			{
				ID = "pageCachingSettings",
				Title = @"Page Caching Settings",
				Width = 500,
				Height = 300,
				Layout = "fit",
				Modal = true
			};

			var formPanel = new FormPanel { Padding = 5 };
			var formLayout = new FormLayout();
			formPanel.ContentControls.Add(formLayout);
			window.Items.Add(formPanel);

			var chkEnableCache = new Checkbox
			{
				ID = "chkEnableCache",
				FieldLabel = @"Enable page cache?",
				LabelSeparator = "",
				Checked = contentItem.GetPageCachingEnabled()
			};
			formLayout.Anchors.Add(new Anchor(chkEnableCache));

			var tmeCacheDuration = new TimeField
			{
				ID = "tmeCacheDuration",
				FieldLabel = @"Cache duration",
				Width = 80,
				SelectedTime = contentItem.GetPageCachingDuration()
			};
			formLayout.Anchors.Add(new Anchor(tmeCacheDuration));

			Button btnSave = new Button { Text = @"Save" };
			window.Buttons.Add(btnSave);
			btnSave.Listeners.Click.Handler = string.Format(
				"stbStatusBar.showBusy(); Ext.net.DirectMethods.PageCaching.SavePageCachingSettings('{0}', Ext.getCmp('{1}').getValue(), Ext.getCmp('{2}').getValue(), {{ url: '{4}', success: function() {{ stbStatusBar.setStatus({{ text: 'Saved page caching settings', iconCls: '', clear: true }}); }} }}); {3}.close();",
				id, chkEnableCache.ClientID, tmeCacheDuration.ClientID, window.ClientID, Engine.AdminManager.GetAdminDefaultUrl());

			Button btnCancel = new Button { Text = @"Cancel" };
			window.Buttons.Add(btnCancel);
			btnCancel.Listeners.Click.Handler = string.Format("{0}.close();", window.ClientID);

			window.Render(pnlContainer, RenderMode.RenderTo);
		}

		[DirectMethod]
		public void SavePageCachingSettings(int id, bool enabled, string duration)
		{
			TimeSpan durationTime = TimeSpan.Parse(duration);

			ContentItem contentItem = Engine.Persister.Get(id);
			contentItem.SetPageCachingEnabled(enabled);
			contentItem.SetPageCachingDuration(durationTime);
			Engine.Persister.Save(contentItem);
		}

		[DirectMethod]
		public void DeleteCachedPage(string id)
		{
			if (string.IsNullOrEmpty(id))
				return;

			ContentItem contentItem = Engine.Persister.Get(Convert.ToInt32(id));
			Engine.Resolve<ICachingService>().DeleteCachedPage(contentItem);
		}
	}
}