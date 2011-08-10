using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Newtonsoft.Json.Linq;

namespace Zeus.Admin
{
	public abstract class MenuLoaderBase<TPlugin> : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";
			string nodeId = context.Request["node"];

			if (!string.IsNullOrEmpty(nodeId))
			{
                //use abs value due to placement folders needing to use negative value of their parent so sorting will work
				ContentItem selectedItem = Context.Persister.Get(Math.Abs(Convert.ToInt32(nodeId)));

				//if (context.User.Identity.Name != "administrator")
				//	filter = new CompositeSpecification<ContentItem>(new PageSpecification<ContentItem>(), filter);

                List<BaseMenuItem> menuItems = CreateMenuItems(selectedItem);

				JArray serializedMenuItems = JArray.FromObject(menuItems.Select(mi => GetObjectForJsonSerialization(mi)));
				context.Response.Write(serializedMenuItems);
				context.Response.End();
			}
		}

		private static object GetObjectForJsonSerialization(Component baseMenuItem)
		{
			if (baseMenuItem is MenuSeparator)
				return "-";

			if (baseMenuItem is MenuItem)
			{
				MenuItem menuItem = (MenuItem)baseMenuItem;
				if (menuItem.Menu != null && menuItem.Menu.Primary != null)
				{
					return new
					{
						text = GetMenuItemText(menuItem),
						icon = menuItem.IconUrl,
						handler = menuItem.Handler,
						menu = new
						{
							items = menuItem.Menu.Primary.Items.Select(mi => GetObjectForJsonSerialization(mi))
						},
						disabled = !menuItem.Enabled
					};
				}
				return new
				{
					text = GetMenuItemText(menuItem),
					icon = menuItem.IconUrl,
					handler = menuItem.Handler,
					disabled = !menuItem.Enabled
				};
			}

			return null;
		}

		private static string GetMenuItemText(MenuItem menuItem)
		{
			if (menuItem.ControlStyle.Font.Bold)
				return "<b>" + menuItem.Text + "</b>";

			return menuItem.Text;
		}

		private List<BaseMenuItem> CreateMenuItems(ContentItem currentItem)
		{
			List<BaseMenuItem> result = new List<BaseMenuItem>();

			bool first = false;
			foreach (ActionPluginGroupAttribute actionPluginGroup in Context.AdminManager.GetActionPluginGroups())
			{
				foreach (TPlugin plugin in GetPlugins(actionPluginGroup.Name))
				{
					if (IsApplicable(plugin, currentItem))
					{
						if (first)
							result.Add(new MenuSeparator());

						MenuItem menuItem = GetMenuItem(plugin, currentItem);
						menuItem.Enabled = IsEnabled(plugin, currentItem);

						// Check if this is the default plugin for this content item.
						if (IsDefault(plugin, currentItem))
							menuItem.ControlStyle.Font.Bold = true;

						result.Add(menuItem);

						first = false;
					}
				}
				if (result.Any())
					first = true;
			}

			return result;
		}

		protected abstract IEnumerable<TPlugin> GetPlugins(string groupName);
		protected abstract bool IsApplicable(TPlugin plugin, ContentItem item);
        protected abstract bool IsDefault(TPlugin plugin, ContentItem item);
		protected abstract MenuItem GetMenuItem(TPlugin plugin, ContentItem item);
		protected abstract bool IsEnabled(TPlugin plugin, ContentItem item);

		public bool IsReusable
		{
			get { return false; }
		}
	}
}