using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coolite.Ext.Web;
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
				ContentItem selectedItem = Context.Persister.Get(Convert.ToInt32(nodeId));

				//if (context.User.Identity.Name != "administrator")
				//	filter = new CompositeSpecification<ContentItem>(new PageSpecification<ContentItem>(), filter);
				List<BaseMenuItem> menuItems = CreateMenuItems(selectedItem);

				JArray serializedMenuItems = JArray.FromObject(menuItems.Select(mi => GetObjectForJsonSerialization(mi)));
				context.Response.Write(serializedMenuItems);
				context.Response.End();
			}
		}

		private static object GetObjectForJsonSerialization(BaseMenuItem baseMenuItem)
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
						text = menuItem.Text,
						icon = menuItem.IconUrl,
						handler = menuItem.Handler,
						menu = new
						{
							items = menuItem.Menu.Primary.Items.Select(mi => GetObjectForJsonSerialization(mi))
						}
					};
				}
				return new
				{
					text = menuItem.Text,
					icon = menuItem.IconUrl,
					handler = menuItem.Handler,
				};
			}

			return null;
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
						if (!IsEnabled(plugin, currentItem))
							menuItem.Enabled = false;

						result.Add(menuItem);
					}
					first = false;
				}
				first = true;
			}

			return result;
		}

		protected abstract IEnumerable<TPlugin> GetPlugins(string groupName);
		protected abstract bool IsApplicable(TPlugin plugin, ContentItem item);
		protected abstract MenuItem GetMenuItem(TPlugin plugin, ContentItem item);
		protected abstract bool IsEnabled(TPlugin plugin, ContentItem item);

		public bool IsReusable
		{
			get { return false; }
		}
	}
}