using System.Linq;
using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Security;

namespace Zeus.Admin.Plugins.ManageZones
{
	public class ManageZonesMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		protected override bool AvailableByDefault
		{
			get { return false; }
		}

		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Change; }
		}

		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.ManageZones.ManageZonesUserControl.ascx") }; }
		}

		public override int SortOrder
		{
			get { return 5; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!contentItem.IsPage)
				return false;

			return base.IsApplicable(contentItem);
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			ContentType definition = Context.ContentTypes.GetContentType(contentItem.GetType());
			if (!definition.AvailableZones.Any())
				return false;

			return base.IsEnabled(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ Ext.net.DirectMethods.ManageZones.OpenZonesPanel({0}, {{ url : '{1}' }}); }}",
				contentItem.ID, Context.AdminManager.GetAdminDefaultUrl());
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			var menuItem = new MenuItem
			{
				Text = "Manage Zones",
				IconUrl = Utility.GetCooliteIconUrl(Icon.ApplicationSideBoxes),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}