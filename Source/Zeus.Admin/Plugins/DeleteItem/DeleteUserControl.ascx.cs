using System;
using System.Linq;
using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.DeleteItem
{
	[AjaxMethodProxyID(Alias = "Delete", IDMode = AjaxMethodProxyIDMode.Alias)]
	public partial class DeleteUserControl : PluginUserControlBase
	{
		[AjaxMethod]
		public void DeleteItems(string ids)
		{
			if (string.IsNullOrEmpty(ids))
				return;

			try
			{
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
			catch (Exception ex)
			{
				//Engine.Resolve<IErrorHandler>().Notify(ex);
				//csvException.IsValid = false;
				//csvException.Text = ex.ToString();
			}
		}
	}
}