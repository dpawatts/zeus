using Ext.Net;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Preview
{
	[ActionPluginGroup("Preview", 5)]
	public class PreviewMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "Preview"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Read; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!contentItem.IsPage)
				return false;

			return base.IsApplicable(contentItem);
		}

		public override bool IsDefault(ContentItem contentItem)
		{
			return true;
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Preview', '{0}'); }}", contentItem.Url);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Preview",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Magnifier),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}