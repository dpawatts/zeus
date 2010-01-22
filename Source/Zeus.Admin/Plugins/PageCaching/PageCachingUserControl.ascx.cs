using System;
using Coolite.Ext.Web;
using Zeus.Web.Caching;

namespace Zeus.Admin.Plugins.PageCaching
{
	[AjaxMethodProxyID(Alias = "PageCaching", IDMode = AjaxMethodProxyIDMode.Alias)]
	public partial class PageCachingUserControl : PluginUserControlBase
	{
		[AjaxMethod]
		public void DeleteCachedPage(string id)
		{
			if (string.IsNullOrEmpty(id))
				return;

			ContentItem contentItem = Engine.Persister.Get(Convert.ToInt32(id));
			Engine.Resolve<ICachingService>().DeleteCachedPage(contentItem);
		}
	}
}