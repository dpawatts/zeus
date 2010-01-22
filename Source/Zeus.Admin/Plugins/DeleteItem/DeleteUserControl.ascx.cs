using System;
using System.Linq;
using Ext.Net;

namespace Zeus.Admin.Plugins.DeleteItem
{
	[DirectMethodProxyID(Alias = "Delete", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class DeleteUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void DeleteItems(string ids)
		{
			if (string.IsNullOrEmpty(ids))
				return;

			string[] nodeIDsTemp = ids.Split(',');
			var nodeIDs = nodeIDsTemp.Select(s => Convert.ToInt32(s));
			if (!nodeIDs.Any())
				return;

			ContentItem parent = Engine.Persister.Get(nodeIDs.First()).Parent;
			foreach (int id in nodeIDs)
			{
				ContentItem item = Engine.Persister.Get(id);
				Zeus.Context.Persister.Delete(item);
			}

			if (parent != null)
				Refresh(parent);
			else
				Refresh(Zeus.Context.UrlParser.StartPage);
		}
	}
}