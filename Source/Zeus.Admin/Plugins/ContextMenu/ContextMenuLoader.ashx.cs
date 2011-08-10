using System.Collections.Generic;
using System.Linq;
using Ext.Net;

namespace Zeus.Admin.Plugins.ContextMenu
{
	public class ContextMenuLoader : MenuLoaderBase<IContextMenuPlugin>
	{
		protected override IEnumerable<IContextMenuPlugin> GetPlugins(string groupName)
		{
			return Context.Current.ResolveAll<IContextMenuPlugin>()
				.Where(p => p.GroupName == groupName)
				.OrderBy(p => p.SortOrder);
		}

		protected override bool IsApplicable(IContextMenuPlugin plugin, ContentItem item)
		{
			return plugin.IsApplicable(item);
        }

		protected override bool IsDefault(IContextMenuPlugin plugin, ContentItem item)
		{
			return plugin.IsDefault(item);
		}

		protected override MenuItem GetMenuItem(IContextMenuPlugin plugin, ContentItem item)
		{
			return plugin.GetMenuItem(item);
		}

		protected override bool IsEnabled(IContextMenuPlugin plugin, ContentItem item)
		{
			return plugin.IsEnabled(item);
		}
	}
}