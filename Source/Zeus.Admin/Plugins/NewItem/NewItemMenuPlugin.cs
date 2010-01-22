using System.Linq;
using System.Web.UI;
using Ext.Net;
using Zeus.BaseLibrary.Web.UI;
using Zeus.ContentTypes;
using Zeus.Security;

[assembly: WebResource("Zeus.Admin.Plugins.NewItem.Resources.Ext.ux.zeus.NewItemContextMenuPlugin.js", "text/javascript")]
[assembly: WebResource("Zeus.Admin.Plugins.NewItem.Resources.Ext.ux.zeus.NewItemGridMenuPlugin.js", "text/javascript")]

namespace Zeus.Admin.Plugins.NewItem
{
	public class NewItemMenuPlugin : MenuPluginBase, IContextMenuPlugin, IGridMenuPlugin
	{
		#region Properties

		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Create; }
		}

		string[] IContextMenuPlugin.RequiredScripts
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.NewItem.Resources.Ext.ux.zeus.NewItemContextMenuPlugin.js") }; }
		}

		string[] IGridMenuPlugin.RequiredScripts
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.NewItem.Resources.Ext.ux.zeus.NewItemGridPlugin.js") }; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		#endregion

		public override bool IsEnabled(ContentItem contentItem)
		{
			if (!base.IsEnabled(contentItem))
				return false;

			// Check that this content item has allowed children
			IContentTypeManager contentTypeManager = Context.ContentTypes;
			ContentType contentType = contentTypeManager.GetContentType(contentItem.GetType());
			if (!contentTypeManager.GetAllowedChildren(contentType, null, Context.Current.WebContext.User).Any())
				return false;

			return true;
		}

		string IContextMenuPlugin.GetJavascriptHandler(ContentItem contentItem)
		{
			return GetJavascriptHandler(contentItem, "Ext.ux.zeus.NewItemContextMenuPlugin");
		}

		private string GetJavascriptHandler(ContentItem contentItem, string clientPluginClass)
		{
			return string.Format("function() {{ new top.{0}('New', '{1}').execute(); }}", clientPluginClass,
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.NewItem.Default.aspx") + "?selected=" + contentItem.Path);
		}

		private MenuItem GetMenuItem(ContentItem contentItem, string clientPluginClass)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "New",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageAdd),
				Handler = GetJavascriptHandler(contentItem, clientPluginClass)
			};

			// Add child menu items for types that can be created under the current item.
			IContentTypeManager manager = Context.Current.Resolve<IContentTypeManager>();
			var childTypes = manager.GetAllowedChildren(manager.GetContentType(contentItem.GetType()), null, Context.Current.WebContext.User);
			if (childTypes.Any())
			{
				Menu childMenu = new Menu();
				menuItem.Menu.Add(childMenu);
				foreach (ContentType child in childTypes)
				{
					MenuItem childMenuItem = new MenuItem
					{
						Text = child.Title,
						IconUrl = child.IconUrl,
						Handler = string.Format("function() {{ new top.{0}('New {1}', '{2}').execute(); }}", clientPluginClass,
							child.Title, Context.AdminManager.GetEditNewPageUrl(contentItem, child, null, CreationPosition.Below))
					};
					childMenu.Items.Add(childMenuItem);
				}
			}

			return menuItem;
		}

		MenuItem IContextMenuPlugin.GetMenuItem(ContentItem contentItem)
		{
			return GetMenuItem(contentItem, "Ext.ux.zeus.NewItemContextMenuPlugin");
		}

		MenuItem IGridMenuPlugin.GetMenuItem(ContentItem contentItem)
		{
			return GetMenuItem(contentItem, "Ext.ux.zeus.NewItemGridMenuPlugin");
		}
	}
}