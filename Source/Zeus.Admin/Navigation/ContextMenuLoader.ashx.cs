using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coolite.Ext.Web;
using Newtonsoft.Json.Linq;

namespace Zeus.Admin.Navigation
{
	public class ContextMenuLoader : IHttpHandler
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
				MenuItem menuItem = (MenuItem) baseMenuItem;
				return new
				{
					text = menuItem.Text,
					icon = menuItem.IconUrl,
					handler = menuItem.Handler
				};
			}

			return null;
		}

		private static List<BaseMenuItem> CreateMenuItems(ContentItem currentItem)
		{
			List<BaseMenuItem> result = new List<BaseMenuItem>();

			bool first = false;
			foreach (ActionPluginGroupAttribute actionPluginGroup in Context.AdminManager.GetActionPluginGroups())
			{
				foreach (IActionPlugin actionPlugin in Context.AdminManager.GetActionPlugins(actionPluginGroup.Name))
				{
					if (actionPlugin.IsApplicable(currentItem))
					{
						if (first)
							result.Add(new MenuSeparator());

						MenuItem menuItem = actionPlugin.GetMenuItem(currentItem);
						if (!actionPlugin.IsEnabled(currentItem))
							menuItem.Enabled = false;

						result.Add(menuItem);
					}
					first = false;
				}
				first = true;
			}

			return result;
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}
